using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public class FGBuildableResourceExtractor : FGBuildableFactory
    {
        [SaveProperty("mExtractResourceNode")]
        public ObjectReference ExtractResourceNode { get; set; }

        [SaveProperty("mCurrentExtractProgress")]
        public float CurrentExtractProgress { get; set; }

        [SaveProperty("mOutputInventory")]
        public ObjectReference OutputInventory { get; set; }
    }
}
