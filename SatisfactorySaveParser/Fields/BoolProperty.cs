using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class BoolProperty : ISerializedField
    {
        public bool Value { get; set; }

        public override string ToString()
        {
            return $"bool: {Value}";
        }

        public static BoolProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new BoolProperty();

            result.Value = reader.ReadInt16() > 0;

            return result;
        }
    }
}
