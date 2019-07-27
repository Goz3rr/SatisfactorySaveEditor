using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableDockingStation : FGBuildableFactory
    {
        [SaveProperty("mInventory")]
        public ObjectReference Inventory { get; set; }

        [SaveProperty("mFuelInventory")]
        public ObjectReference FuelInventory { get; set; }

        [SaveProperty("mDockedActor")]
        public ObjectReference DockedActor { get; set; }

        [SaveProperty("mHasDockedActor")]
        public bool HasDockedActor { get; set; }

        [SaveProperty("mIsInLoadMode")]
        public bool IsInLoadMode { get; set; }

        [SaveProperty("mIsLoadUnloading")]
        public bool IsLoadUnloading { get; set; }
    }
}
