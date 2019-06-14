using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Schematics
{
    [SaveObjectClass("/Game/FactoryGame/Schematics/Progression/BP_SchematicManager.BP_SchematicManager_C")]
    public class SchematicManager : SaveActor
    {
        [SaveProperty("mShipLandTimeStampSave")]
        public float ShipLandTimeStampSave { get; set; }
    }
}
