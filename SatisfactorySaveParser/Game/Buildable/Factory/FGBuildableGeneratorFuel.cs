using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableGeneratorFuel : FGBuildableGenerator
    {
        [SaveProperty("mFuelInventory")]
        public ObjectReference FuelInventory { get; set; }

        /// <summary>
        ///     Amount left of the currently burned piece of fuel. In megawatt seconds (MWs).
        /// </summary>
        [SaveProperty("mCurrentFuelAmount")]
        public float CurrentFuelAmount { get; set; }

        /// <summary>
        ///     Amount left of the currently loaded supplemental resource. In Liters ( 1 Liquid inventory item = 1 Liter )
        /// </summary>
        [SaveProperty("mCurrentSupplementalAmount")]
        public float CurrentSupplementalAmount { get; set; }

        /// <summary>
        ///     Used so clients know how if they have available fuel or not. Could be removed later if we start syncing the production indicator state
        /// </summary>
        [SaveProperty("mHasFuelCached")] // TODO mHasFuleCached
        public bool HasFuelCached { get; set; }

        /// <summary>
        ///     Like the mHasFuelCached - Used to notify clients if there is enough supplemental resource available
        /// </summary>
        [SaveProperty("mHasSupplementalCached")]
        public bool HasSupplementalCached { get; set; }

        /// <summary>
        ///     Type of the currently burned piece of fuel.
        /// </summary>
        [SaveProperty("mCurrentFuelClass")]
        public ObjectReference CurrentFuelClass { get; set; }
    }
}
