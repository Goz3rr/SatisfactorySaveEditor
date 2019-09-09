using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableSplitterSmart : FGBuildableAttachmentSplitter
    {
        [SaveProperty("mSortRules")]
        public List<FSplitterSortRule> SortRules { get; } = new List<FSplitterSortRule>();

        [SaveProperty("mLastItem")]
        public FInventoryItem LastItem { get; set; }

        [SaveProperty("mItemToLastOutputMap")]
        public Dictionary<ObjectReference, byte> ItemToLastOutputMap { get; } = new Dictionary<ObjectReference, byte>();

        [SaveProperty("mLastOutputIndex")]
        public int LastOutputIndex { get; set; }
    }
}
