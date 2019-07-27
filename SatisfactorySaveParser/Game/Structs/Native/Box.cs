using System.IO;
using System.Numerics;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Box")]
    public class Box : GameStruct
    {
        public override string StructName => "Box";
        public override int SerializedLength => 25;

        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        public byte UnknownByte { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Min = reader.ReadVector3();
            Max = reader.ReadVector3();

            UnknownByte = reader.ReadByte();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Min);
            writer.Write(Max);
            writer.Write(UnknownByte);
        }
    }
}
