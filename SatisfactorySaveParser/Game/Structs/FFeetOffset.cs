namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("FeetOffset")]
    public class FFeetOffset : GameStruct
    {
        public override string StructName => "FeetOffset";

        [StructProperty("FeetIndex")]
        public byte FeetIndex { get; set; }

        [StructProperty("FeetName")]
        public string FeetName { get; set; }

        [StructProperty("OffsetZ")]
        public float OffsetZ { get; set; }

        [StructProperty("ShouldShow")]
        public bool ShouldShow { get; set; }
    }
}
