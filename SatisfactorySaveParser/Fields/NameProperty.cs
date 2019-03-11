using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class NameProperty : ISerializedField
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return $"name: {Value}";
        }

        public static NameProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new NameProperty();

            var size = reader.ReadInt32();
            var unk2 = reader.ReadInt32();
            var unk3 = reader.ReadByte();

            var before = reader.BaseStream.Position;
            result.Value = reader.ReadLengthPrefixedString();
            var after = reader.BaseStream.Position;

            if (before + size != after)
            {
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
            }

            return result;
        }
    }
}
