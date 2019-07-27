using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("CompletedResearch")]
    public class FCompletedResearch : GameStruct
    {
        public override string StructName => "CompletedResearch";

        [StructProperty("researchRecipe")]
        public ObjectReference ResearchRecipe { get; set; }

        [StructProperty("RewardHasBeenClaimed")]
        public bool RewardHasBeenClaimed { get; set; }
    }
}
