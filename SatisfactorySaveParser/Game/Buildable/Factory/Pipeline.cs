using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/Pipeline/Build_Pipeline.Build_Pipeline_C")]
    public class Pipeline : FGBuildablePipeBase
    {
        /// <summary>
        ///     Simulation data.
        /// </summary>
        [SaveProperty("mFluidBox")]
        public FFluidBox FluidBox { get; set; }
    }
}
