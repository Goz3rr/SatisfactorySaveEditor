using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ArrayProperty : SerializedProperty
    {
        public const string TypeName = nameof(ArrayProperty);

        public string Type { get; set; }
        public List<SerializedProperty> Elements { get; set; } = new List<SerializedProperty>();

        public ArrayProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"array of {Type}";
        }

        public static ArrayProperty Parse(string propertyName, BinaryReader reader, int size, out int overhead)
        {
            var result = new ArrayProperty(propertyName)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            byte unk2 = reader.ReadByte();
            Trace.Assert(unk2 == 0);

            switch (result.Type)
            {
                case StructProperty.TypeName:
                    {
                        // TODO
                        reader.ReadBytes(size);
                    }
                    break;
                case ObjectProperty.TypeName:
                    {
                        int count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            string obj1 = reader.ReadLengthPrefixedString();
                            string obj2 = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new ObjectProperty(null, obj1, obj2));
                        }
                    }
                    break;
                case IntProperty.TypeName:
                    {
                        int count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            int element = reader.ReadInt32();
                            result.Elements.Add(new IntProperty(null, element));
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
