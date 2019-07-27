using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("ResearchCost")]
    public class FResearchCost : GameStruct
    {
        public override string StructName => "ResearchCost";

        [StructProperty("researchRecipe")]
        public ObjectReference ResearchRecipe { get; set; }

        [StructProperty("Cost")]
        public List<FItemAmount> Cost { get; } = new List<FItemAmount>();
    }
}
