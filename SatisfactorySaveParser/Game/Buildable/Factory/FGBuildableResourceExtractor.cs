using System;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public class FGBuildableResourceExtractor : FGBuildableFactory
    {
        /// <summary>
        ///     How much time we have left of the start up time for the extraction process (mExtractStartupTime). 
        /// </summary>
        [SaveProperty("mExtractStartupTimer")]
        public float ExtractStartupTimer { get; set; }

        /// <summary>
        ///     DEPRICATED - Only used for old save support. Use mExtractableResource instead.
        ///     The resource node we want to extract from.
        /// </summary>
        [SaveProperty("mExtractResourceNode"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public ObjectReference ExtractResourceNode { get; set; }

        [SaveProperty("mExtractableResource")]
        public ObjectReference ExtractableResource { get; set; }

        /// <summary>
        ///     Current extract progress in the range [0, 1]
        /// </summary>
        [SaveProperty("mCurrentExtractProgress")]
        public float CurrentExtractProgress { get; set; }

        /// <summary>
        ///     Our output inventory
        /// </summary>
        [SaveProperty("mOutputInventory")]
        public ObjectReference OutputInventory { get; set; }
    }
}
