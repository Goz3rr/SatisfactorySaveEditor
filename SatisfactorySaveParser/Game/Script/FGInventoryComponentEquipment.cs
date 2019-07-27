using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGInventoryComponentEquipment")]
    public class FGInventoryComponentEquipment : FGInventoryComponent
    {
        [SaveProperty("mEquipmentInSlot")]
        public ObjectReference EquipmentInSlot { get; set; }

        [SaveProperty("mActiveEquipmentIndex")]
        public int ActiveEquipmentIndex { get; set; }
    }
}
