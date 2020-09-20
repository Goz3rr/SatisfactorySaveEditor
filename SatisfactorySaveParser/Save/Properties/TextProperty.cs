using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties
{
    public class TextProperty : SerializedProperty, ITextPropertyValue
    {
        public const string TypeName = nameof(TextProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(TextEntry);
        public override object BackingObject => Text;

        public override int SerializedLength => 5 + Text.SerializedLength;

        /// <summary>
        ///     History Type
        /// </summary>
        public ETextHistoryType HistoryType { get; set; }

        public TextEntry Text { get; set; }

        public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Text {PropertyName}: {Text}";
        }

        public static TextEntry ParseTextEntry(BinaryReader reader)
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
                            SourceFormat = (BaseTextEntry)ParseTextEntry(reader)
                        };

                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Arguments.Add(new ArgumentFormat()
                            {
                                Name = reader.ReadLengthPrefixedString(),
                                ValueType = (EFormatArgumentType)reader.ReadByte(),
                                Value = ParseTextEntry(reader)
                            });

                        }
                        return result;
                    }
                case ETextHistoryType.None:
                    {
                        return new NoneTextEntry(flags);
                    }
                default:
                    throw new NotImplementedException($"Unknown ETextHistoryType {historyType}");
            }
        }

        public static TextProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new TextProperty(propertyName, index);

            result.ReadPropertyGuid(reader);
            result.Text = ParseTextEntry(reader);

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            WritePropertyGuid(writer);
            writer.Write(Text.Flags);
            writer.Write((byte)Text.HistoryType);

        }
    }
}
