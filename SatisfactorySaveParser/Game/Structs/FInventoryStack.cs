using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("InventoryStack")]
    public class FInventoryStack : GameStruct
    {
        public override string StructName => "InventoryStack";

        [StructProperty("Item")]
        public FInventoryItem Item { get; set; }

        [StructProperty("NumItems")]
        public int NumItems { get; set; }
    }
}
