using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class StrArrayValue : IStrPropertyValue, IArrayElement
    {
        public string Value { get; set; }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
