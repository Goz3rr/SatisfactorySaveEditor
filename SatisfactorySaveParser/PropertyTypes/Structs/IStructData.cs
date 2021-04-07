using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public interface IStructData
    {
        int SerializedLength { get; }
        string Type { get; }

        void Serialize(BinaryWriter writer, int buildVersion);
    }
}
