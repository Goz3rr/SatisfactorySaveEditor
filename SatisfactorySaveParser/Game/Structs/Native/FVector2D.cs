using System.IO;
using System.Numerics;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Vector2D")]
    public class FVector2D : GameStruct
    {
        public override string StructName => "Vector2D";
        public override int SerializedLength => 8;

        public Vector2 Data { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Data = reader.ReadVector2();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
