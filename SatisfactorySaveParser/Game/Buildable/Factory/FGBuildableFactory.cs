using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableFactory : FGBuildable
    {
        [SaveProperty("mPowerInfo")]
        public ObjectReference PowerInfo { get; set; }

        [SaveProperty("mTimeSinceStartStopProducing")]
        public float TimeSinceStartStopProducing { get; set; }

        [SaveProperty("mCurrentPotential")]
        public float CurrentPotential { get; set; }

        [SaveProperty("mPendingPotential")]
        public float PendingPotential { get; set; }

        [SaveProperty("mIsProductionPaused")]
        public bool IsProductionPaused { get; set; }

        [SaveProperty("mInventoryPotential")]
        public ObjectReference InventoryPotential { get; set; }

        [SaveProperty("mIsProducing")]
        public bool IsProducing { get; set; }
    }
}
