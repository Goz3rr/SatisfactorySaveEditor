using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/PortableMiner/BP_PortableMiner.BP_PortableMiner_C")]
    public class PortableMiner : SaveActor
    {
        [SaveProperty("mExtractResourceNode")]
        public ObjectReference ExtractResourceNode { get; set; }

        [SaveProperty("mOutputInventory")]
        public ObjectReference OutputInventory { get; set; }
    }
}
