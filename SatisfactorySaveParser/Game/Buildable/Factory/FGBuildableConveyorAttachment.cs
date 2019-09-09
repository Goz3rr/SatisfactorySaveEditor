using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableConveyorAttachment : FGBuildableFactory
    {
        [SaveProperty("mBufferInventory")]
        public ObjectReference BufferInventory { get; set; }
    }
}
