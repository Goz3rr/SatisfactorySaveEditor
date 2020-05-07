using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class EnumArrayValue : IEnumPropertyValue, IArrayElement
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
