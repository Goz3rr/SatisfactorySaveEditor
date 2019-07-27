using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("TimeTableStop")]
    public class FTimeTableStop : GameStruct
    {
        public override string StructName => "TimeTableStop";

        [StructProperty("Station")]
        public ObjectReference Station { get; set; }

        [StructProperty("Duration")]
        public float Duration { get; set; }
    }
}
