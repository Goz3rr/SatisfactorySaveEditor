using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector4D : IStructData
    {
        public int SerializedLength => 16;
        public string Type => "Vector4";
        public Vector4 Data { get; set; }

        public Vector4D(BinaryReader reader)
        {
            Data = reader.ReadVector4();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Data);
        }
    }
}
