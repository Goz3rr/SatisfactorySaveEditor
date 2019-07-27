using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    public abstract class FGBuildableTrainPlatform : FGBuildableFactory
    {
        [SaveProperty("mRailroadTrack")]
        public ObjectReference RailroadTrack { get; set; }

        [SaveProperty("mDockedRailroadVehicle")]
        public ObjectReference DockedRailroadVehicle { get; set; }

        [SaveProperty("mStationDockingMaster")]
        public ObjectReference StationDockingMaster { get; set; }

        [SaveProperty("mIsOrientationReversed")]
        public bool IsOrientationReversed { get; set; }

        [SaveProperty("SavedDockingStatus")]
        public ETrainPlatformDockingStatus SavedDockingStatus { get; set; }
    }
}
