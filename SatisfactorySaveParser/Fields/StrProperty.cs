using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class StrProperty : ISerializedField
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return $"str: {Value}";
        }

        public static StrProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new StrProperty();

            var unk3 = reader.ReadByte();

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
