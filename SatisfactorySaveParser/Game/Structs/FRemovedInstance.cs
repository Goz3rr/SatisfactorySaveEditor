namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("RemovedInstance")]
    public class FRemovedInstance : GameStruct
    {
        public override string StructName => "RemovedInstance";

        [StructProperty("Transform")]
        public FTransform Transform { get; set; }
    }
}
