using System.Collections.Generic;

using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    public abstract class FGBuildableRailroadStation : FGBuildableTrainPlatform
    {
        [SaveProperty("mStationDockingStatus")]
        public EStationDockingStatus StationDockingStatus { get; set; }

        [SaveProperty("mDockedPlatformList")]
        public List<ObjectReference> DockedPlatformList { get; } = new List<ObjectReference>();

        [SaveProperty("mDockingLocomotive")]
        public ObjectReference DockingLocomotive { get; set; }

        [SaveProperty("mTrackPosition")]
        public FRailroadTrackPosition TrackPosition { get; set; }
    }
}
