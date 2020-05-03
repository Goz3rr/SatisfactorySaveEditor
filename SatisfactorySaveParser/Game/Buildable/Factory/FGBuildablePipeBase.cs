using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Pipeline for transferring liquid and gases to factory buildings.
    /// </summary>
    public abstract class FGBuildablePipeBase : FGBuildable
    {
        [SaveProperty("mSplineData")]
        public List<FSplinePointData> SplineData { get; } = new List<FSplinePointData>();
    }
}
