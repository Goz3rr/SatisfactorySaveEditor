using System;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class LinearColor : IStructData
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public int SerializedLength => 16;
        public string Type => "LinearColor";

        public LinearColor(BinaryReader reader)
        {
            R = reader.ReadSingle();
            G = reader.ReadSingle();
            B = reader.ReadSingle();
            A = reader.ReadSingle();
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
            writer.Write(A);
        }
    }
}
