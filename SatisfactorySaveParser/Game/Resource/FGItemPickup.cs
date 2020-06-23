using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Resource
{
    public abstract class FGItemPickup : SaveActor
    {
        [SaveProperty("mPickupItems")]
        public FInventoryStack PickupItems { get; set; }

        [SaveProperty("mUpdatedOnDayNr")]
        public int UpdatedOnDayNr { get; set; }

        [SaveProperty("mItemState")]
        public EItemState ItemState { get; set; }

        [SaveProperty("mSavedNumItems")]
        public int SavedNumItems { get; set; }

        [SaveProperty("mNumRespawns")]
        public int NumRespawns { get; set; }
    }
}
