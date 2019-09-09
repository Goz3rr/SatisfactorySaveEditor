using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("RecipeAmountStruct")]
    public class RecipeAmountStruct : GameStruct
    {
        public override string StructName => "RecipeAmountStruct";

        // TODO: wat

        [StructProperty("Recipe_3_9675A43D4FEC0E33CE84EA9FCEF0E903")]
        public ObjectReference Recipe { get; set; }

        [StructProperty("Amount_6_262F181A4A294617FCD1F496A451BA13")]
        public int Amount { get; set; }
    }
}
