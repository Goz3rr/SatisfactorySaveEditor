using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Base class for variable length pipe supports
    /// </summary>
    public abstract class FGBuildablePipePart : FGBuildableFactory
    {
        /// <summary>
        ///     This supports length.
        /// </summary>
        [SaveProperty("mLength")]
        public float Length { get; set; }
    }
}
