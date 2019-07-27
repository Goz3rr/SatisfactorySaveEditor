using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("ItemAmount")]
    public class FItemAmount : GameStruct
    {
        public override string StructName => "ItemAmount";

        [StructProperty("ItemClass")]
        public ObjectReference ItemClass { get; set; }

        [StructProperty("amount")]
        public int Amount { get; set; }
    }
}
