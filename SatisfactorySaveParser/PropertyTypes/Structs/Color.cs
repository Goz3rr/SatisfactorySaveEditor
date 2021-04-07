using System;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class Color : IStructData
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public int SerializedLength => 4;
        public string Type => "Color";

        public Color(BinaryReader reader)
        {
            B = reader.ReadByte();
            G = reader.ReadByte();
            R = reader.ReadByte();
            A = reader.ReadByte();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(B);
            writer.Write(G);
            writer.Write(R);
            writer.Write(A);
        }
    }
}
