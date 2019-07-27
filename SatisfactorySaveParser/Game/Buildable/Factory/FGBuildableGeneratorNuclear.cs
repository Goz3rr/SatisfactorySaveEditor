using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableGeneratorNuclear : FGBuildableGeneratorFuel
    {
        [SaveProperty("mOutputInventory")]
        public ObjectReference OutputInventory { get; set; }

        [SaveProperty("mWasteLeftFromCurrentFuel")]
        public int WasteLeftFromCurrentFuel { get; set; }
    }
}
