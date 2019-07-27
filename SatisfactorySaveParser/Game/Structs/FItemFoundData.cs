namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("ItemFoundData")]
    public class FItemFoundData : GameStruct
    {
        public override string StructName => "ItemFoundData";

        [StructProperty("WasFound")]
        public bool WasFound { get; set; }
    }
}
