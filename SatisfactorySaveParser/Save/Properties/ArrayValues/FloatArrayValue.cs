using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class FloatArrayValue : IFloatPropertyValue, IArrayElement
    {
        public float Value { get; set; }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
