using System.Collections.Generic;

using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

// Buildables\FGBuildableRailroadStation.h

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    /// <summary>
    ///     Base class for rail road stations. Not to be confused railroad docking stations.
    /// </summary>
    public abstract class FGBuildableRailroadStation : FGBuildableTrainPlatform
    {
        [SaveProperty("mStationDockingStatus")]
        public EStationDockingStatus StationDockingStatus { get; set; }

        /// <summary>
        ///     When docked, this station will fill this array with every potential platform in its tail. 1 for each train segment
        /// </summary>
        [SaveProperty("mDockedPlatformList")]
        public List<ObjectReference> DockedPlatformList { get; } = new List<ObjectReference>();

        /// <summary>
        ///     Reference to the docked locomotive.
        /// </summary>
        [SaveProperty("mDockingLocomotive")]
        public ObjectReference DockingLocomotive { get; set; }

        [SaveProperty("mTrackPosition")]
        public FRailroadTrackPosition TrackPosition { get; set; }
    }
}
