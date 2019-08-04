using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    // TODO
    public class FResearchTime : GameStruct
    {
        public override string StructName => throw new System.NotImplementedException();

        public ObjectReference ResearchRecipe { get; set; }

        public float ResearchCompleteTimestamp { get; set; }
    }
}
