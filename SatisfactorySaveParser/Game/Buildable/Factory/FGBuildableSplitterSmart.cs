using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public abstract class FGBuildableSplitterSmart : FGBuildableAttachmentSplitter
    {
        [SaveProperty("mSortRules")]
        public List<FSplitterSortRule> SortRules { get; } = new List<FSplitterSortRule>();
    }
}
