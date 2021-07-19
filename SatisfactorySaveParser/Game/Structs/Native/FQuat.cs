using System.IO;
using System.Numerics;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("Quat")]
    public class FQuat : GameStruct
    {
        public override string StructName => "Quat";
        public override int SerializedLength => 16;

        public Quaternion Data { get; set; }

        public override void Deserialize(BinaryReader reader, int buildVersion)
        {
            Data = reader.ReadQuat();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Data);
        }
    }
}
