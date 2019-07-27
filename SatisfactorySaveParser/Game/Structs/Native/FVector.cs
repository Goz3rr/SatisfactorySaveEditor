using System.IO;
using System.Numerics;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Vector")]
    public class FVector : GameStruct
    {
        public override string StructName => "Vector";
        public override int SerializedLength => 12;

        public Vector3 Data { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Data = reader.ReadVector3();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
