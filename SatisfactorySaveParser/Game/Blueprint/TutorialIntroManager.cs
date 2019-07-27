using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TutorialIntroManager.BP_TutorialIntroManager_C")]
    public class TutorialIntroManager : SaveActor
    {
        [SaveProperty("mTradingPostBuilt")]
        public bool TradingPostBuilt { get; set; }

        [SaveProperty("mPendingTutorial")]
        public EIntroTutorialSteps PendingTutorial { get; set; }

        [SaveProperty("mHasCompletedIntroTutorial")]
        public bool HasCompletedIntroTutorial { get; set; }

        [SaveProperty("mHasCompletedIntroSequence")]
        public bool HasCompletedIntroSequence { get; set; }

        [SaveProperty("mTradingPost")]
        public ObjectReference TradingPost { get; set; }

        [SaveProperty("mDidPickUpIronOre")]
        public bool DidPickUpIronOre { get; set; }

        [SaveProperty("mDidDismantleDropPod")]
        public bool DidDismantleDropPod { get; set; }

        [SaveProperty("mDidEquipStunSpear")]
        public bool DidEquipStunSpear { get; set; }

        [SaveProperty("mDidStep1Upgrade")]
        public bool DidStep1Upgrade { get; set; }
        
        [SaveProperty("mDidStep2Upgrade")]
        public bool DidStep2Upgrade { get; set; }

        [SaveProperty("mDidStep3Upgrade")]
        public bool DidStep3Upgrade { get; set; }

        [SaveProperty("mDidStep4Upgrade")]
        public bool DidStep4Upgrade { get; set; }

        [SaveProperty("mDidStep5Upgrade")]
        public bool DidStep5Upgrade { get; set; }

        [SaveProperty("mTradingPostLevel")]
        public int TradingPostLevel { get; set; }
    }
}
