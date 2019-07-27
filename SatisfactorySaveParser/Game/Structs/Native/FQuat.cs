using System.IO;
using System.Numerics;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Quat")]
    public class FQuat : GameStruct
    {
        public override string StructName => "Quat";
        public override int SerializedLength => 16;

        public Vector4 Data { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Data = reader.ReadVector4();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
