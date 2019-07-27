using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Resource
{
    [SaveObjectClass("/Game/FactoryGame/Resource/BP_ResourceDeposit.BP_ResourceDeposit_C")]
    public class ResourceDeposit : FGResourceNode
    {
        [SaveProperty("mResourceDepositTableIndex")]
        public int ResourceDepositTableIndex { get; set; }

        [SaveProperty("mIsEmptied")]
        public bool IsEmptied { get; set; }

        [SaveProperty("mMineAmount")]
        public int MineAmount { get; set; }
    }
}
