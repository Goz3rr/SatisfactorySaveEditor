using System.IO;

namespace SatisfactorySaveParser.Game.Structs.Native
{
    [GameStruct("InventoryItem")]
    public class FInventoryItem : GameStruct
    {
        public override string StructName => "InventoryItem";
        public override int SerializedLength => 4 + ItemType.GetSerializedLength() + Unknown2.GetSerializedLength() + Unknown3.GetSerializedLength();

        public int Unknown1 { get; set; }
        public string ItemType { get; set; }
        public string Unknown2 { get; set; }
        public string Unknown3 { get; set; }

        public override void Deserialize(BinaryReader reader)
        {
            Unknown1 = reader.ReadInt32();
            ItemType = reader.ReadLengthPrefixedString();
            Unknown2 = reader.ReadLengthPrefixedString();
            Unknown3 = reader.ReadLengthPrefixedString();
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write(Unknown1);
            writer.WriteLengthPrefixedString(ItemType);
            writer.WriteLengthPrefixedString(Unknown2);
            writer.WriteLengthPrefixedString(Unknown3);
        }
    }
}
