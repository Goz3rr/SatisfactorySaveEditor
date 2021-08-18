using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("DateTime")]
    public class FDateTime : GameStruct
    {
        public override string StructName => "DateTime";
        public override int SerializedLength => 8;

        public long Timestamp { get; set; }

        public override void Deserialize(BinaryReader reader, int buildVersion)
        {
            Timestamp = reader.ReadInt64();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Timestamp);
        }
    }
}
