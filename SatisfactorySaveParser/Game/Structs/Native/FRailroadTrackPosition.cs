using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("RailroadTrackPosition")]
    public class FRailroadTrackPosition : GameStruct
    {
        public override string StructName => "RailroadTrackPosition";
        public override int SerializedLength => Root.GetSerializedLength() + InstanceName.GetSerializedLength() + 8;

        public string Root { get; set; }
        public string InstanceName { get; set; }
        public float Offset { get; set; }
        public float Forward { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Root = reader.ReadLengthPrefixedString();
            InstanceName = reader.ReadLengthPrefixedString();
            Offset = reader.ReadSingle();
            Forward = reader.ReadSingle();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(Root);
            writer.WriteLengthPrefixedString(InstanceName);
            writer.Write(Offset);
            writer.Write(Forward);
        }
    }
}
