using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Base for pipe attachments such as pumps and junctions.
    /// </summary>
    public abstract class FGBuildablePipelineAttachment : FGBuildableFactory
    {
        /// <summary>
        ///     Fluid box belonging to this integrant
        /// </summary>
        [SaveProperty("mFluidBox")]
        public FFluidBox FluidBox { get; set; }
    }
}
