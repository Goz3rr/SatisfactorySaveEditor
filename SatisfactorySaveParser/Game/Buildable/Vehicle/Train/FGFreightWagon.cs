using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle.Train
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Vehicle/Train/Wagon/BP_FreightWagon.BP_FreightWagon_C")]
    public class FGFreightWagon : FGRailroadVehicle
    {
        [SaveProperty("mStorageInventory")]
        public ObjectReference StorageInventory { get; set; }
    }
}
