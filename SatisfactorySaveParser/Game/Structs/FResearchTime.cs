using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    public class FResearchTime : GameStruct
    {
        public override string StructName => "";

        public ObjectReference ResearchRecipe { get; set; }

        public float ResearchCompleteTimestamp { get; set; }
    }
}
