using System;
using System.IO;

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
