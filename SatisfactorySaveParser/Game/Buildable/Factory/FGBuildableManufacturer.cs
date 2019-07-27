using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableManufacturer : FGBuildableFactory
    {
        [SaveProperty("mCurrentManufacturingProgress")]
        public float CurrentManufacturingProgress { get; set; }

        [SaveProperty("mInputInventory")]
        public ObjectReference InputInventory { get; set; }

        [SaveProperty("mOutputInventory")]
        public ObjectReference OutputInventory { get; set; }

        [SaveProperty("mCurrentRecipe")]
        public ObjectReference CurrentRecipe { get; set; }
    }
}
