using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Resource
{
    public abstract class FGItemPickup : SaveActor
    {
        [SaveProperty("mPickupItems")]
        public FInventoryStack PickupItems { get; set; }
    }
}
