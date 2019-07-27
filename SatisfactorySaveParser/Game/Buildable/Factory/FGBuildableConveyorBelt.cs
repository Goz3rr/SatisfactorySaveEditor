using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public class FGBuildableConveyorBelt : FGBuildableConveyorBase
    {
        [SaveProperty("mSplineData")]
        public List<FSplinePointData> SplineData { get; } = new List<FSplinePointData>();
    }
}
