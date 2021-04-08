using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class FloatArrayValue : IFloatPropertyValue, IArrayElement
    {
        public Type BackingType => typeof(float);

        public float Value { get; set; }

        public static FloatArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new FloatArrayValue()
            {
                Value = reader.ReadSingle()
            };
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
