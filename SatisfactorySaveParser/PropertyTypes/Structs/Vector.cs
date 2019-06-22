using SatisfactorySaveParser.Structures;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector : IStructData
    {
        public int SerializedLength => 12;
        public string Type => "Vector";
        public Vector3 Data { get; set; }

        public Vector(BinaryReader reader)
        {
            Data = new Vector3()
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle()
            };
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
