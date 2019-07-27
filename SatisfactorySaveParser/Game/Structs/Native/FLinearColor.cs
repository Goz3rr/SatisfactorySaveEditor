using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("LinearColor")]
    public class FLinearColor : GameStruct
    {
        public override string StructName => "LinearColor";
        public override int SerializedLength => 16;

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            R = reader.ReadSingle();
            G = reader.ReadSingle();
            B = reader.ReadSingle();
            A = reader.ReadSingle();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
            writer.Write(A);
        }
    }
}
