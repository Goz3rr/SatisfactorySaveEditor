using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableGeneratorGeoThermal : FGBuildableGenerator
    {
        [SaveProperty("mExtractResourceNode")]
        public ObjectReference ExtractResourceNode { get; set; }
    }
}
