using SatisfactorySaveParser.Structures;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector : Vector3, IStructData
    {
        public int SerializedLength => 12;
        public string Type => "Vector";

        public Vector(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(this);
        }
    }
}
