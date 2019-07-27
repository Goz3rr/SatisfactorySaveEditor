using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableGeneratorFuel : FGBuildableGenerator
    {
        [SaveProperty("mFuelInventory")]
        public ObjectReference FuelInventory { get; set; }

        [SaveProperty("mCurrentFuelAmount")]
        public float mCurrentFuelAmount { get; set; }

        [SaveProperty("mHasFuleCached")]
        public bool HasFuelCached { get; set; }

        [SaveProperty("mCurrentFuelClass")]
        public ObjectReference CurrentFuelClass { get; set; }
    }
}
