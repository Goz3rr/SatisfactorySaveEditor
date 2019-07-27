using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGInventoryComponent")]
    public class FGInventoryComponent : SaveComponent
    {
        [SaveProperty("mAdjustedSizeDiff")]
        public int AdjustedSizeDiff { get; set; }

        [SaveProperty("mInventoryStacks")]
        public List<FInventoryStack> InventoryStacks { get; } = new List<FInventoryStack>();

        [SaveProperty("mArbitrarySlotSizes")]
        public List<int> ArbitrarySlotSizes { get; } = new List<int>();

        [SaveProperty("mAllowedItemDescriptors")]
        public List<ObjectReference> AllowedItemDescriptors { get; } = new List<ObjectReference>();

        [SaveProperty("mCanBeRearrange")]
        public bool CanBeRearrange { get; set; }
    }
}
