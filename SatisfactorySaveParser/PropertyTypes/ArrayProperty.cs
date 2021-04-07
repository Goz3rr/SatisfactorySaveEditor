using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ArrayProperty : SerializedProperty
    {
        public const string TypeName = nameof(ArrayProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength
        {
            get
            {
                switch (Type)
                {
                    case StructProperty.TypeName:
                        return StructProperty.GetSerializedArrayLength(Elements.Cast<StructProperty>().ToArray());
                    case ObjectProperty.TypeName:
                        return 4 + Elements.Cast<ObjectProperty>().Sum(obj => obj.LevelName.GetSerializedLength() + obj.PathName.GetSerializedLength());
                    case IntProperty.TypeName:
                        return Elements.Count * 4 + 4;
                    case ByteProperty.TypeName:
                        return 4 + Elements.Count;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        ///     String representation of the Property type this array consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the array
        /// </summary>
        public List<SerializedProperty> Elements { get; set; } = new List<SerializedProperty>();

        public ArrayProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"array of {Type}";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            if (writeHeader)
            {
                base.Serialize(writer, buildVersion, writeHeader);
            }

            using (var ms = new MemoryStream())
            using (var msWriter = new BinaryWriter(ms))
            {
                switch (Type)
                {
                    case StructProperty.TypeName:
                        {
                            StructProperty.SerializeArray(msWriter, Elements.Cast<StructProperty>().ToArray(), buildVersion);
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
                    case IntProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<IntProperty>())
                            {
                                msWriter.Write(prop.Value);
                            }
                        }
                        break;
                    case ByteProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach(var prop in Elements.Cast<ByteProperty>())
                            {
                                msWriter.Write(byte.Parse(prop.Value));
                            }
                        }
                        break;
                    case EnumProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<EnumProperty>())
                            {
                                msWriter.WriteLengthPrefixedString(prop.Name);
                            }
                        }
                        break;
                    case StrProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<StrProperty>())
                            {
                                msWriter.WriteLengthPrefixedString(prop.Value);
                            }
                        }
                        break;
                    case FloatProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<FloatProperty>())
                            {
                                msWriter.Write(prop.Value);
                            }
                        }
                        break;
                    case TextProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<TextProperty>())
                            {
                                prop.Serialize(msWriter, buildVersion, false);
                            }
                        }
                        break;
                    case InterfaceProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<InterfaceProperty>())
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

                writer.Write(bytes.Length);
                writer.Write(Index);

                writer.WriteLengthPrefixedString(Type);
                writer.Write((byte)0);

                writer.Write(bytes);
            }
        }

        public static ArrayProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead, int buildVersion)
        {
            var result = new ArrayProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            byte unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            switch (result.Type)
            {
                case StructProperty.TypeName:
                    {
                        result.Elements.AddRange(StructProperty.ParseArray(reader, buildVersion));
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
                case ByteProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for(var i = 0; i < count; i++)
                        {
                            var value = reader.ReadByte();
                            result.Elements.Add(new ByteProperty($"Element {i}") { Type = "None", Value = value.ToString() });
                        }
                    }
                    break;
                case EnumProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var str = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new EnumProperty($"Element {i}") { Type = str.Split(':')[0], Name = str });
                        }
                    }
                    break;
                case StrProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var str = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new StrProperty($"Element {i}") { Value = str });
                        }
                    }
                    break;
                case FloatProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var value = reader.ReadSingle();
                            result.Elements.Add(new FloatProperty($"Element {i}") { Value = value });
                        }
                    }
                    break;
                case TextProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(TextProperty.Parse($"Element {i}", 0, reader, buildVersion, true));
                        }
                    }
                    break;
                case InterfaceProperty.TypeName:
                    {
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var obj1 = reader.ReadLengthPrefixedString();
                            var obj2 = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new InterfaceProperty($"Element {i}", obj1, obj2));
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException($"Parsing an array of {result.Type} is not yet supported.");
            }

            return result;
        }
    }
}
