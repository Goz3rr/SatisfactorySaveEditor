using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public interface IStructData
    {
        int SerializedLength { get; }

        void Serialize(BinaryWriter writer);
    }
}
