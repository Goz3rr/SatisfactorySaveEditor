using System;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class ArrayProperty : ISerializedField
    {
        public string Type { get; set; }
        public List<(string, string)> Elements { get; set; } = new List<(string, string)>();

        public override string ToString()
        {
            return $"array";
        }

        public static ArrayProperty Parse(string fieldName, BinaryReader reader, int size, out int overhead)
        {
            var result = new ArrayProperty();

            result.Type = reader.ReadLengthPrefixedString();
            overhead = result.Type.Length + 6;

            if (result.Type == "StructProperty")
            {
                // TODO
                reader.ReadBytes(size+1);
                return result;
            }

            if (result.Type != "ObjectProperty") throw new NotImplementedException();

            byte unk2 = reader.ReadByte();
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                string obj1 = reader.ReadLengthPrefixedString();
                string obj2 = reader.ReadLengthPrefixedString();
                result.Elements.Add((obj1, obj2));
            }

            return result;
        }
    }
}
