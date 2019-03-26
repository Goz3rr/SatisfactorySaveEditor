using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Vector : IStructData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public int SerializedLength => 12;

        public Vector(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }
    }
}
