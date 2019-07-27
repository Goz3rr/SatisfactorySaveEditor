using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableStorage : FGBuildableFactory
    {
        [SaveProperty("mStorageInventory")]
        public ObjectReference StorageInventory { get; set; }
    }
}
