#pragma warning disable CA1707 // Identifiers should not contain underscores

using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save;

// FGTutorialIntroManager.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TutorialIntroManager.BP_TutorialIntroManager_C")]
    public class TutorialIntroManager : SaveActor
    {
        [SaveProperty("mTradingPostBuilt")]
        public bool TradingPostBuilt { get; set; }

        /// <summary>
        ///     Array of pending tutorial IDs that should be shown when possible ( no other widgets on screen etc )
        /// </summary>
        [SaveProperty("mPendingTutorial")]
        public EIntroTutorialSteps PendingTutorial { get; set; }

        /// <summary>
        ///     Indicates if the player has completed the introduction tutorial
        /// </summary>
        [SaveProperty("mHasCompletedIntroTutorial")]
        public bool HasCompletedIntroTutorial { get; set; }

        /// <summary>
        ///     Indicates that the introduction sequence is done (right now, drop pod sequence)
        /// </summary>
        [SaveProperty("mHasCompletedIntroSequence")]
        public bool HasCompletedIntroSequence { get; set; }

        /// <summary>
        ///     Cached reference of trading post
        /// </summary>
        [SaveProperty("mTradingPost")]
        public ObjectReference TradingPost { get; set; }

        [SaveProperty("mDidPickUpIronOre")]
        public bool DidPickUpIronOre { get; set; }

        /// <summary>
        ///     Checks if we have dismantled the drop pod
        /// </summary>
        [SaveProperty("mDidDismantleDropPod")]
        public bool DidDismantleDropPod { get; set; }

        /// <summary>
        ///     Checks if we equipped the stun spear
        /// </summary>
        [SaveProperty("mDidEquipStunSpear")]
        public bool DidEquipStunSpear { get; set; }

        /// <summary>
        ///     Bool for the step 1 schematic
        /// </summary>
        [SaveProperty("mDidStep1Upgrade")]
        public bool DidStep1Upgrade { get; set; }

        /// <summary>
        ///     Bool for the step 1.5 schematic
        /// </summary>
        [SaveProperty("mDidStep1_5Upgrade")]
        public bool DidStep1_5Upgrade { get; set; }

        /// <summary>
        ///     Bool for the step 2 schematic
        /// </summary>
        [SaveProperty("mDidStep2Upgrade")]
        public bool DidStep2Upgrade { get; set; }

        /// <summary>
        ///     Bool for the step 3 schematic
        /// </summary>
        [SaveProperty("mDidStep3Upgrade")]
        public bool DidStep3Upgrade { get; set; }

        /// <summary>
        ///     Bool for the step 4 schematic
        /// </summary>
        [SaveProperty("mDidStep4Upgrade")]
        public bool DidStep4Upgrade { get; set; }

        /// <summary>
        ///     Bool for the step5 schematic
        /// </summary>
        [SaveProperty("mDidStep5Upgrade")]
        public bool DidStep5Upgrade { get; set; }

        /// <summary>
        ///     The upgrade level we have on our trading post
        /// </summary>
        [SaveProperty("mTradingPostLevel")]
        public int TradingPostLevel { get; set; }
    }
}
