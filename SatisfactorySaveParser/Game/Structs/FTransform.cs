using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("Transform")]
    public class FTransform : GameStruct
    {
        public override string StructName => "Transform";

        [StructProperty("Rotation")]
        public FQuat Rotation { get; set; }

        [StructProperty("Translation")]
        public FVector Translation { get; set; }

        [StructProperty("Scale3D")]
        public FVector Scale3D { get; set; }
    }
}
