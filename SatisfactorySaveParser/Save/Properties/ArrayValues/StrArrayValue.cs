using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class StrArrayValue : IStrPropertyValue, IArrayElement
    {
        public Type BackingType => typeof(string);

        public string Value { get; set; }

        public static StrArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new StrArrayValue()
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
