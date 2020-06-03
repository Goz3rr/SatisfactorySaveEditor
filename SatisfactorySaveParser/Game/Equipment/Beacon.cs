using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/Beacon/BP_Beacon.BP_Beacon_C")]
    public class Beacon : FGConsumableEquipment
    {
        [SaveProperty("mCompassText")]
        public TextEntry CompassText { get; set; }

        [SaveProperty("mCompassColor")]
        public FLinearColor CompassColor { get; set; }
    }
}
