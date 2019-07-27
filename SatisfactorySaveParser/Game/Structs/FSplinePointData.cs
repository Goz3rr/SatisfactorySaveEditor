using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveParser.Game.Structs
{
    public class FSplinePointData : GameStruct
    {
        public override string StructName => throw new System.NotImplementedException();

        [StructProperty("Location")]
        public FVector Location { get; set; }

        [StructProperty("ArriveTangent")]
        public FVector ArriveTangent { get; set; }

        [StructProperty("LeaveTangent")]
        public FVector LeaveTangent { get; set; }
    }
}
