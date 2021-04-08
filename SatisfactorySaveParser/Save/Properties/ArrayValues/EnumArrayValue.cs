using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class EnumArrayValue : IEnumPropertyValue, IArrayElement
    {
        public Type BackingType => typeof(Enum);

        public string Type { get; set; }
        public string Value { get; set; }

        public static EnumArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            var str = reader.ReadLengthPrefixedString();

            return new EnumArrayValue()
            {
                Type = str.Split(':')[0],
                Value = str
            };
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
