using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class ArrayProperty : ISerializedField
    {
        public string Type { get; set; }
        public List<ISerializedField> Elements { get; set; } = new List<ISerializedField>();

        public override string ToString()
        {
            return $"array";
        }

        public static ArrayProperty Parse(string fieldName, BinaryReader reader, int size, out int overhead)
        {
            var result = new ArrayProperty
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            byte unk2 = reader.ReadByte();
            Trace.Assert(unk2 == 0);

            switch (result.Type)
            {
                case "StructProperty":
                    {
                        // TODO
                        reader.ReadBytes(size);
                    }
                    break;
                case "ObjectProperty":
                    {
                        int count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            string obj1 = reader.ReadLengthPrefixedString();
                            string obj2 = reader.ReadLengthPrefixedString();
                            result.Elements.Add(new ObjectProperty(obj1, obj2));
                        }
                    }
                    break;
                case "IntProperty":
                    {
                        int count = reader.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            int element = reader.ReadInt32();
                            result.Elements.Add(new IntProperty(element));
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
