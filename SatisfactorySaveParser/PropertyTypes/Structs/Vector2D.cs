using System.IO;

using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector2D : IStructData
    {
        public int SerializedLength => 8;
        public string Type => "Vector2D";
        public Vector2 Data { get; set; }

        public Vector2D(BinaryReader reader)
        {
            Data = reader.ReadVector2();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Data);
        }
    }
}
