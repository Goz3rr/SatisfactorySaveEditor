using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TutorialIntroManager.BP_TutorialIntroManager_C")]
    public class TutorialIntroManager : SaveActor
    {
        [SaveProperty("mTradingPostBuilt")]
        public bool TradingPostBuilt { get; set; }
    }
}
