using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/CA_Merger/Build_ConveyorAttachmentMerger.Build_ConveyorAttachmentMerger_C")]
    public class FGBuildableAttachmentMerger : FGBuildableConveyorAttachment
    {
        [SaveProperty("mCurrentInputIndex")]
        public int CurrentInputIndex { get; set; }

        [SaveProperty("mCurrentInventoryIndex")]
        public int CurrentInventoryIndex { get; set; }
    }
}
