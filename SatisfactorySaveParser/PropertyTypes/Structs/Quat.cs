using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Quat : IStructData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public int SerializedLength => 16;
        public string Type => "Quat";

        public Quat(BinaryReader reader)
        {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
            W = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
            writer.Write(W);
        }
    }
}
