using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public class ByteArrayValue : IBytePropertyValue, IArrayElement
    {
        public string EnumType { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public string EnumValue { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public byte ByteValue { get; set; }

        public bool IsEnum => false;

        public void ArraySerialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
