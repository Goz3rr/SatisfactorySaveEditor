using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Recipes.Research
{
    /// <summary>
    ///     The research manager handles everything research related 
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/Recipes/Research/BP_ResearchManager.BP_ResearchManager_C")]
    public class ResearchManager : SaveActor
    {
        [SaveProperty("mResearchCosts")]
        public List<FResearchCost> ResearchCosts { get; } = new List<FResearchCost>();

        [SaveProperty("mCompletedResearch")]
        public List<FCompletedResearch> CompletedResearch { get; } = new List<FCompletedResearch>();
    }
}
