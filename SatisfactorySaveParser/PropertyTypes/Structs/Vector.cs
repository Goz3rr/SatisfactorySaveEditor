using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector : IStructData
    {
        public int SerializedLength => 12;
        public string Type => "Vector";
        public Vector3 Data { get; set; }

        public Vector(BinaryReader reader)
        {
            Data = reader.ReadVector3();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Data);
        }
    }
}
