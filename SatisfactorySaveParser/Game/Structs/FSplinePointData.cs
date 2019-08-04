using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("SplinePointData")]
    public class FSplinePointData : GameStruct
    {
        public override string StructName => "SplinePointData";

        [StructProperty("Location")]
        public FVector Location { get; set; }

        [StructProperty("ArriveTangent")]
        public FVector ArriveTangent { get; set; }

        [StructProperty("LeaveTangent")]
        public FVector LeaveTangent { get; set; }
    }
}
