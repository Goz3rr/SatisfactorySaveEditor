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

            var unk3 = reader.ReadByte();

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
