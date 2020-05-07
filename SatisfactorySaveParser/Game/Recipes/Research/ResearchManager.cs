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

        [SaveProperty("mUnlockedResearchTrees")]
        public List<ObjectReference> UnlockedResearchTrees { get; } = new List<ObjectReference>();

        /// <summary>
        ///     What research has been conducted and is complete.
        /// </summary>
        [SaveProperty("mCompletedResearch")]
        public List<FResearchData> CompletedResearch { get; } = new List<FResearchData>();

        /// <summary>
        ///     Used save the current ongoing research, saved research is restarted on load
        /// </summary>
        [SaveProperty("mSavedOngoingResearch")]
        public List<FResearchTime> SavedOngoingResearch { get; } = new List<FResearchTime>();
    }
}
