using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Schematics.Progression
{
    [SaveObjectClass("/Game/FactoryGame/Schematics/Progression/BP_SchematicManager.BP_SchematicManager_C")]
    public class SchematicManager : SaveActor
    {
        [SaveProperty("mAvailableSchematics")]
        public List<ObjectReference> AvailableSchematics { get; } = new List<ObjectReference>();

        [SaveProperty("mPurchasedSchematics")]
        public List<ObjectReference> PurchasedSchematics { get; } = new List<ObjectReference>();

        [SaveProperty("mPaidOffSchematic")]
        public List<FSchematicCost> PaidOffSchematic { get; } = new List<FSchematicCost>();

        [SaveProperty("mActiveSchematic")]
        public ObjectReference ActiveSchematic { get; set; }

        [SaveProperty("mShipLandTimeStampSave")]
        public float ShipLandTimeStampSave { get; set; }
    }
}
