using System;
using System.Collections.Generic;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class ArrayProperty : ISerializedField
    {
        public List<(string, string)> Elements { get; set; } = new List<(string, string)>();

        public override string ToString()
        {
            return $"array";
        }

        public static ArrayProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new ArrayProperty();

            int size = reader.ReadInt32();
            int unk = reader.ReadInt32();
            string childType = reader.ReadLengthPrefixedString();

            if (childType != "ObjectProperty") throw new NotImplementedException();

            byte unk2 = reader.ReadByte();
            var before = reader.BaseStream.Position;
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                string obj1 = reader.ReadLengthPrefixedString();
                string obj2 = reader.ReadLengthPrefixedString();
                result.Elements.Add((obj1, obj2));
            }
            var after = reader.BaseStream.Position;

            if (before + size != after)
            {
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
            }

            return result;
        }
    }
}
