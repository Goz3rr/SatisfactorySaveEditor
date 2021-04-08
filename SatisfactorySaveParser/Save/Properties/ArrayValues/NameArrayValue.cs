using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class NameArrayValue : INamePropertyValue, IArrayElement
    {
        public Type BackingType => typeof(string);

        public string Value { get; set; }

        public static NameArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new NameArrayValue()
            {
                Value = reader.ReadLengthPrefixedString()
            };
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
