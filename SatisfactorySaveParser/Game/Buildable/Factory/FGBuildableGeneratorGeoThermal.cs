using System;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableGeneratorGeoThermal : FGBuildableGenerator
    {
        /// <summary>
        ///     DEPRICATED - Use mExtractableResource instead. This exists for save functionality
        ///     This is the geyser this generator is placed on
        /// </summary>
        [SaveProperty("mExtractResourceNode"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public ObjectReference ExtractResourceNode { get; set; }

        [SaveProperty("mExtractableResource")]
        public ObjectReference ExtractableResource { get; set; }
    }
}
