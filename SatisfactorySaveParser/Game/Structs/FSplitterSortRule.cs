using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("SplitterSortRule")]
    public class FSplitterSortRule : GameStruct
    {
        public override string StructName => "SplitterSortRule";

        [StructProperty("ItemClass")]
        public ObjectReference ItemClass { get; set; }

        [StructProperty("OutputIndex")]
        public int OutputIndex { get; set; }
    }
}
