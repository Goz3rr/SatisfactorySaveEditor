using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Color")]
    public class FColor : GameStruct
    {
        public override string StructName => "Color";
        public override int SerializedLength => 4;

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            B = reader.ReadByte();
            G = reader.ReadByte();
            R = reader.ReadByte();
            A = reader.ReadByte();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(B);
            writer.Write(G);
            writer.Write(R);
            writer.Write(A);
        }
    }
}
