using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_GameState.BP_GameState_C")]
    public class FGGameState : SaveActor
    {
        [SaveProperty("mTimeSubsystem")]
        public ObjectReference TimeSubsystem { get; set; }

        [SaveProperty("mStorySubsystem")]
        public ObjectReference StorySubsystem { get; set; }

        [SaveProperty("mRailroadSubsystem")]
        public ObjectReference RailroadSubsystem { get; set; }

        [SaveProperty("mCircuitSubsystem")]
        public ObjectReference CircuitSubsystem { get; set; }

        [SaveProperty("mRecipeManager")]
        public ObjectReference RecipeManager { get; set; }

        [SaveProperty("mSchematicManager")]
        public ObjectReference SchematicManager { get; set; }

        [SaveProperty("mGamePhaseManager")]
        public ObjectReference GamePhaseManager { get; set; }

        [SaveProperty("mResearchManager")]
        public ObjectReference ResearchManager { get; set; }

        [SaveProperty("mTutorialIntroManager")]
        public ObjectReference TutorialIntroManager { get; set; }

        [SaveProperty("mActorRepresentationManager")]
        public ObjectReference ActorRepresentationManager { get; set; }

        [SaveProperty("mMapManager")]
        public ObjectReference MapManager { get; set; }

        [SaveProperty("mScannableResources")]
        public List<ObjectReference> ScannableResources { get; } = new List<ObjectReference>();

        [SaveProperty("mVisitedMapAreas")]
        public List<ObjectReference> VisitedMapAreas { get; } = new List<ObjectReference>();

        [SaveProperty("mCheatNoCost")]
        public bool CheatNoCost { get; set; }

        [SaveProperty("mCheatNoPower")]
        public bool CheatNoPower { get; set; }

        [SaveProperty("mIsMapUnlocked")]
        public bool IsMapUnlocked { get; set; }

        [SaveProperty("mNumAdditionalInventorySlots")]
        public int NumAdditionalInventorySlots { get; set; }

        [SaveProperty("mIsBuildingEfficiencyUnlocked")]
        public bool IsBuildingEfficiencyUnlocked { get; set; }

        [SaveProperty("mIsBuildingOverclockUnlocked")]
        public bool IsBuildingOverclockUnlocked { get; set; }

        [SaveProperty("mIsTradingPostBuilt")]
        public bool IsTradingPostBuilt { get; set; }

        [SaveProperty("mHasInitalTradingPostLandAnimPlayed")]
        public bool HasInitalTradingPostLandAnimPlayed { get; set; }

        [SaveProperty("mIsSpaceElevatorBuilt")]
        public bool IsSpaceElevatorBuilt { get; set; }

        [SaveProperty("mPlayDurationWhenLoaded")]
        public int PlayDurationWhenLoaded { get; set; }

        [SaveProperty("mReplicatedSessionName")]
        public string ReplicatedSessionName { get; set; }

        [SaveProperty("mForceAddHubPartOnSpawn")]
        public bool ForceAddHubPartOnSpawn { get; set; }

        [SaveProperty("mNumAdditionalArmEquipmentSlots")]
        public int NumAdditionalArmEquipmentSlots { get; set; }
    }
}
