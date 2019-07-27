using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle
{
    public abstract class FGWheeledVehicle : FGVehicle
    {
        [SaveProperty("mCurrentFuelAmount")]
        public float CurrentFuelAmount { get; set; }

        [SaveProperty("mIsLoadingVehicle")]
        public bool IsLoadingVehicle { get; set; }

        [SaveProperty("mIsUnloadingVehicle")]
        public bool IsUnloadingVehicle { get; set; }

        [SaveProperty("mCurrentFuelClass")]
        public ObjectReference CurrentFuelClass { get; set; }

        [SaveProperty("mIsSimulated")]
        public bool IsSimulated { get; set; }

        [SaveProperty("mFuelInventory")]
        public ObjectReference FuelInventory { get; set; }

        [SaveProperty("mStorageInventory")]
        public ObjectReference StorageInventory { get; set; }

        [SaveProperty("mTargetNodeLinkedList")]
        public ObjectReference TargetNodeLinkedList { get; set; }

        [SaveProperty("mIsPathVisible")]
        public bool IsPathVisible { get; set; }
    }
}
