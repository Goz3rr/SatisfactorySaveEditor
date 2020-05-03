using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Base class for variable length pipe supports
    /// </summary>
    public abstract class FGBuildablePipelineSupport : FGBuildablePoleBase
    {
        /// <summary>
        ///     This supports length.
        /// </summary>
        [SaveProperty("mLength")]
        public float Length { get; set; }

        /// <summary>
        ///     This supports length.
        /// </summary>
        [SaveProperty("mVerticalAngle")]
        public float VerticalAngle { get; set; }
    }
}
