using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

// Buildables\FGBuildableRailroadTrack.h

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    /// <summary>
    ///     A piece of train track, it has a spline and to ends.
    /// </summary>
    public abstract class FGBuildableRailroadTrack : FGBuildable
    {
        /// <summary>
        ///     Spline data saved in a compact form for saving and replicating. All the vectors are in local space.
        /// </summary>
        [SaveProperty("mSplineData")]
        public List<FSplinePointData> mSplineData { get; } = new List<FSplinePointData>();

        /// <summary>
        ///     This tracks connection component.
        /// </summary>
        [SaveProperty("mConnections")]
        public ObjectReference[] Connections { get; } = new ObjectReference[2];

        /// <summary>
        ///     Was this track created and is owned by a platform.
        /// </summary>
        [SaveProperty("mIsOwnedByPlatform")]
        public bool IsOwnedByPlatform { get; set; }
    }
}
