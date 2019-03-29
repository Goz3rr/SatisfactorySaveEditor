using System;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Box : IStructData
    {
        public float X1 { get; set; }
        public float X2 { get; set; }
        public float Y1 { get; set; }
        public float Y2 { get; set; }
        public float Z1 { get; set; }
        public float Z2 { get; set; }
        public byte UnknownByte { get; set; }

        public int SerializedLength => 25;
        public string Type => "Box";

        public Box(BinaryReader reader)
        {
            X1 = reader.ReadSingle();
            Y1 = reader.ReadSingle();
            Z1 = reader.ReadSingle();

            X2 = reader.ReadSingle();
            Y2 = reader.ReadSingle();
            Z2 = reader.ReadSingle();

            UnknownByte = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(X1);
            writer.Write(Y1);
            writer.Write(Z1);
            writer.Write(X2);
            writer.Write(Y2);
            writer.Write(Z2);
            writer.Write(UnknownByte);
        }
    }
}
