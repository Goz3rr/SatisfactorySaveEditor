using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Resource
{
    [SaveObjectClass("/Game/FactoryGame/Resource/BP_ResourceNode.BP_ResourceNode_C")]
    public class FGResourceNode : SaveActor
    {
        [SaveProperty("mResourcesLeft")]
        public int ResourcesLeft { get; set; }

        [SaveProperty("mIsOccupied")]
        public bool IsOccupied { get; set; }

        [SaveProperty("mRevealedOnMapBy")]
        public List<ObjectReference> RevealedOnMapBy { get; } = new List<ObjectReference>();

        [SaveProperty("mDoSpawnParticle")]
        public bool DoSpawnParticle { get; set; }
    }
}
