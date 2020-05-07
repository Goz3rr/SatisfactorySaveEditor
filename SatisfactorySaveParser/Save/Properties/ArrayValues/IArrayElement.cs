using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public interface IArrayElement
    {
        Type BackingType { get; }

        void ArraySerialize(BinaryWriter writer);
    }
}
