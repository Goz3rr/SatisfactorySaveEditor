using System.IO;

namespace SatisfactorySaveParser.Save.Properties.ArrayValues
{
    public interface IArrayElement
    {
        void ArraySerialize(BinaryWriter writer);
    }
}
