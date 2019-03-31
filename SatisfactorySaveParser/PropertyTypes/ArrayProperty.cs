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
                        return 4 + Elements.Cast<ObjectProperty>().Sum(obj => obj.Str1.GetSerializedLength() + obj.Str2.GetSerializedLength());
                    case IntProperty.TypeName:
                        return Elements.Count * 4 + 4;
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

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            if (writeHeader)
            {
                base.Serialize(writer, writeHeader);
            }

            using (var ms = new MemoryStream())
            using (var msWriter = new BinaryWriter(ms))
            {
                switch (Type)
                {
                    case StructProperty.TypeName:
                        {
                            StructProperty.SerializeArray(msWriter, Elements.Cast<StructProperty>().ToArray());
                        }
                        break;
                    case ObjectProperty.TypeName:
                        {
                            msWriter.Write(Elements.Count);
                            foreach (var prop in Elements.Cast<ObjectProperty>())
                            {
                                msWriter.WriteLengthPrefixedString(prop.Str1);
                                msWriter.WriteLengthPrefixedString(prop.Str2);
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
                    default:
                        throw new NotImplementedException();
                }

                var bytes = ms.ToArray();

                writer.Write(bytes.Length);
                writer.Write(Index);

                writer.WriteLengthPrefixedString(Type);
                writer.Write((byte)0);

                writer.Write(bytes);
            }
        }

        public static ArrayProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
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
                        result.Elements.AddRange(StructProperty.ParseArray(reader));
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
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}
