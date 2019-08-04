using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Crate/BP_Crate.BP_Crate_C")]
    public class FGCrate : SaveActor
    {
        [SaveProperty("mInventory")]
        public ObjectReference Inventory { get; set; }
    }
}
