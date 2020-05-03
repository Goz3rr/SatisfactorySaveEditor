using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Pipeline for transferring liquid and gases to factory buildings.
    /// </summary>
    public abstract class FGBuildablePipeReservoir : FGBuildableFactory
    {
        /// <summary>
        ///     Fluid box used for flow calculations
        /// </summary>
        [SaveProperty("mFluidBox")]
        public FFluidBox FluidBox { get; set; }
    }
}
