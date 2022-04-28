using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class SetProperty : SerializedProperty
    {
        public const string TypeName = nameof(SetProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => 0;

        public string Type { get; set; }
        public List<SerializedProperty> Elements { get; set; } = new List<SerializedProperty>();

        public SetProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"set: ";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            if (writeHeader)
            {
                base.Serialize(writer, buildVersion, writeHeader);
            }

            int sizeExtra = 0;

            using (var ms = new MemoryStream())
            using (var msWriter = new BinaryWriter(ms))
            {
                switch (Type)
                {
                    case IntProperty.TypeName:
                        {
                            sizeExtra = 4;
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<IntProperty>())
                            {
                                msWriter.Write(prop.Value);
                            }
                        }
                        break;
                    case NameProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<NameProperty>())
                            {
                                msWriter.WriteLengthPrefixedString(prop.Value);
                            }
                        }
                        break;
                    case ObjectProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<ObjectProperty>())
                            {
                                msWriter.WriteLengthPrefixedString(prop.LevelName);
                                msWriter.WriteLengthPrefixedString(prop.PathName);
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Serializing an array of {Type} is not yet supported.");
                }

                var bytes = ms.ToArray();

                writer.Write(bytes.Length + sizeExtra);
                writer.Write(Index);

                writer.WriteLengthPrefixedString(Type);

                writer.Write((byte)0);
                writer.Write(0);

                writer.Write(bytes);
            }
        }

        public static SetProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
        {
            var result = new SetProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            var unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            var unk2 = reader.ReadInt32();
            Trace.Assert(unk2 == 0);

            switch (result.Type)
            {
                case IntProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var value = reader.ReadInt32();
                            result.Elements.Add(new IntProperty($"Element {i}") { Value = value });
                        }
                    }
                    break;
                case NameProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var value = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new NameProperty($"Element {i}") { Value = value });
                        }
                    }
                    break;
                case ObjectProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var obj1 = reader.ReadLengthPrefixedString();
                            var obj2 = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new ObjectProperty($"Element {i}", obj1, obj2));
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException($"Parsing a set of {result.Type} is not yet supported.");
            }


            return result;
        }
    }
}
