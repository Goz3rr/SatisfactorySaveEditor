using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/CA_Splitter/Build_ConveyorAttachmentSplitter.Build_ConveyorAttachmentSplitter_C")]
    public class FGBuildableAttachmentSplitter : FGBuildableConveyorAttachment
    {
        [SaveProperty("mCurrentOutputIndex")]
        public int CurrentOutputIndex { get; set; }

        [SaveProperty("mCurrentInventoryIndex")]
        public int CurrentInventoryIndex { get; set; }
    }
}
