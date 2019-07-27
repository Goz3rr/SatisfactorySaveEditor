using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.World.Benefit
{
    [SaveObjectClass("/Game/FactoryGame/World/Benefit/DropPod/BP_DropPod.BP_DropPod_C")]
    public class DropPod : SaveActor
    {
        [SaveProperty("mHasBeenOpened")]
        public bool HasBeenOpened { get; set; }

        [SaveProperty("mInventory")]
        public ObjectReference Inventory { get; set; }
    }
}
