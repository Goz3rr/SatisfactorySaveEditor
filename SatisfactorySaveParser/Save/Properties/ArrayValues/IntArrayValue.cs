using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class IntArrayValue : IIntPropertyValue, IArrayElement
    {
        public Type BackingType => typeof(int);

        public int Value { get; set; }

        public static IntArrayValue DeserializeArrayValue(BinaryReader reader)
        {
            return new IntArrayValue()
            {
                Value = reader.ReadInt32()
            };
        }

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
