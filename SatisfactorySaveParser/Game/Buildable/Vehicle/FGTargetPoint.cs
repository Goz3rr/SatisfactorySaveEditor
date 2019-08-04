using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Vehicle/BP_VehicleTargetPoint.BP_VehicleTargetPoint_C")]
    public class FGTargetPoint : SaveActor
    {
        [SaveProperty("mNext")]
        public ObjectReference Next { get; set; }

        [SaveProperty("mOwningVehicle")]
        public ObjectReference OwningVehicle { get; set; }

        [SaveProperty("mIsVisible")]
        public bool IsVisible { get; set; }

        [SaveProperty("mWaitTime")]
        public float WaitTime { get; set; }

        [SaveProperty("mTargetSpeed")]
        public int TargetSpeed { get; set; }
    }
}
