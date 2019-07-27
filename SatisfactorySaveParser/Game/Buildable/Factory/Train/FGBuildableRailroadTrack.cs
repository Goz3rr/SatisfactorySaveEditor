using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    public abstract class FGBuildableRailroadTrack : FGBuildable
    {
        [SaveProperty("mSplineData")]
        public List<FSplinePointData> mSplineData { get; } = new List<FSplinePointData>();

        [SaveProperty("mConnections")]
        public ObjectReference[] Connections { get; } = new ObjectReference[2];

        [SaveProperty("mIsOwnedByPlatform")]
        public bool IsOwnedByPlatform { get; set; }
    }
}
