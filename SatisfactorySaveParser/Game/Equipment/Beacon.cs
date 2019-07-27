using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/Beacon/BP_Beacon.BP_Beacon_C")]
    public class Beacon : SaveActor
    {
        [SaveProperty("mCompassText")]
        public TextEntry CompassText { get; set; }
    }
}
