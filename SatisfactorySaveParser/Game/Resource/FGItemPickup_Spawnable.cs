#pragma warning disable CA1707 // Identifiers should not contain underscores

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Resource
{
    [SaveObjectClass("/Game/FactoryGame/Resource/BP_ItemPickup_Spawnable.BP_ItemPickup_Spawnable_C")]
    [SaveObjectClass("/Script/FactoryGame.FGItemPickup_Spawnable")]
    public class FGItemPickup_Spawnable : FGItemPickup
    {
        [SaveProperty("mPlaySpawnEffect")]
        public bool PlaySpawnEffect { get; set; }
    }
}
