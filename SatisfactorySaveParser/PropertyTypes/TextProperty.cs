using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class TextProperty : SerializedProperty
    {
        public const int CultureInvariantChangeBuild = 140822;

        public const string TypeName = nameof(TextProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Text.SerializedLength;

        public TextEntry Text { get; set; }

        public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Text {PropertyName}: {Text}";
        }

        public static void WriteTextEntry(BinaryWriter writer, TextEntry entry, int buildVersion)
        {
            writer.Write(entry.Flags);
            writer.Write((byte)entry.HistoryType);

            switch (entry)
            {
                case BaseTextEntry baseText:
                    {
                        writer.WriteLengthPrefixedString(baseText.Namespace);
                        writer.WriteLengthPrefixedString(baseText.Key);
                        writer.WriteLengthPrefixedString(baseText.Value);
                    }
                    break;
                case ArgumentFormatTextEntry argumentFormatText:
                    {
                        WriteTextEntry(writer, argumentFormatText.SourceFormat, buildVersion);

                        writer.Write(argumentFormatText.Arguments.Count);
                        foreach (var arg in argumentFormatText.Arguments)
                        {
                            writer.WriteLengthPrefixedString(arg.Name);
                            writer.Write((byte)arg.ValueType);
                            WriteTextEntry(writer, arg.Value, buildVersion);
                        }
                    }
                    break;
                case NoneTextEntry noneText:
                    {
                        if (buildVersion >= CultureInvariantChangeBuild && noneText.HasCultureInvariantString.HasValue)
                        {
                            writer.Write(noneText.HasCultureInvariantString.Value ? 1 : 0);

                            if (noneText.HasCultureInvariantString.Value)
                                writer.WriteLengthPrefixedString(noneText.CultureInvariantString);
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException($"Unknown ETextHistoryType {entry.GetType()}");
            }
        }

        private void UpdateTextEntry(TextEntry entry, int buildVersion)
        {
            if (buildVersion >= CultureInvariantChangeBuild)
                return;

            switch (entry)
            {
                case ArgumentFormatTextEntry argFormat:
                    {
                        foreach (var item in argFormat.Arguments)
                            UpdateTextEntry(item.Value, buildVersion);
                    }
                    break;

                case NoneTextEntry none:
                    {
                        none.HasCultureInvariantString = null;
                    }
                    break;
            }
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            // Fix up serializedlength when downgrading
            UpdateTextEntry(Text, buildVersion);

            if (writeHeader)
            {
                base.Serialize(writer, buildVersion, writeHeader);

                writer.Write(SerializedLength);
                writer.Write(Index);
                writer.Write((byte)0);
            }

            WriteTextEntry(writer, Text, buildVersion);
        }

        public static TextEntry ParseTextEntry(BinaryReader reader, int buildVersion)
        {
            var flags = reader.ReadInt32();
            var historyType = (ETextHistoryType)reader.ReadByte();

            switch (historyType)
            {
                case ETextHistoryType.Base:
                    {
                        return new BaseTextEntry(flags)
                        {
                            Namespace = reader.ReadLengthPrefixedString(),
                            Key = reader.ReadLengthPrefixedString(),
                            Value = reader.ReadLengthPrefixedString()
                        };
                    }
                case ETextHistoryType.ArgumentFormat:
                    {
                        var result = new ArgumentFormatTextEntry(flags)
                        {
                            SourceFormat = (BaseTextEntry)ParseTextEntry(reader, buildVersion)
                        };

                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Arguments.Add(new ArgumentFormat()
                            {
                                Name = reader.ReadLengthPrefixedString(),
                                ValueType = (EFormatArgumentType)reader.ReadByte(),
                                Value = ParseTextEntry(reader, buildVersion)
                            });

                        }
                        return result;
                    }
                case ETextHistoryType.None:
                    {
                        var entry = new NoneTextEntry(flags);

                        if (buildVersion >= CultureInvariantChangeBuild)
                        {
                            entry.HasCultureInvariantString = reader.ReadInt32() == 1;

                            if (entry.HasCultureInvariantString.Value)
                                entry.CultureInvariantString = reader.ReadLengthPrefixedString();
                        }

                        return entry;
                    }
                default:
                    throw new NotImplementedException($"Unknown ETextHistoryType {historyType}");
            }
        }

        public static TextProperty Parse(string propertyName, int index, BinaryReader reader, int buildVersion, bool inArray = false)
        {
            var result = new TextProperty(propertyName, index);

            if (!inArray)
            {
                var unk3 = reader.ReadByte();
                Trace.Assert(unk3 == 0);
            }

            result.Text = ParseTextEntry(reader, buildVersion);

            return result;
        }
    }
}
