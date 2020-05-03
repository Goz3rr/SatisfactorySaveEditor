using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Save;

// FGGameState.h

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

        [SaveProperty("mPipeSubsystem")]
        public ObjectReference PipeSubsystem { get; set; }

        [SaveProperty("mUnlockSubsystem")]
        public ObjectReference UnlockSubsystem { get; set; }

        [SaveProperty("mResourceSinkSubsystem")]
        public ObjectReference ResourceSinkSubsystem { get; set; }

        [SaveProperty("mScannableResources")]
        public List<ObjectReference> ScannableResources { get; } = new List<ObjectReference>();

        /// <summary>
        ///     This array keeps track of what map areas have been visited this game
        /// </summary>
        [SaveProperty("mVisitedMapAreas")]
        public List<ObjectReference> VisitedMapAreas { get; } = new List<ObjectReference>();

        /// <summary>
        ///     All items we have picked up that also are relevant to know if we picked up
        /// </summary>
        [SaveProperty("mPickedUpItems")]
        public List<ObjectReference> PickedUpItems { get; } = new List<ObjectReference>();

        /// <summary>
        ///     Cheat bool for having no cost for stuff
        /// </summary>
        [SaveProperty("mCheatNoCost")]
        public bool CheatNoCost { get; set; }

        /// <summary>
        ///     Cheat bool for not requiring power
        /// </summary>
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

        /// <summary>
        ///     There can only be one trading post in the game, so we keep track it here so that we also can replicate it to client
        /// </summary>
        [SaveProperty("mIsTradingPostBuilt")]
        public bool IsTradingPostBuilt { get; set; }

        /// <summary>
        ///     The first time you build a trading post we want the landing animation to play
        /// </summary>
        [SaveProperty("mHasInitalTradingPostLandAnimPlayed")]
        public bool HasInitalTradingPostLandAnimPlayed { get; set; }

        /// <summary>
        ///     There can only be one tow truck in the game, so we keep track it here so that we also can replicate it to client
        /// </summary>
        [SaveProperty("mIsSpaceElevatorBuilt")]
        public bool IsSpaceElevatorBuilt { get; set; }

        /// <summary>
        ///     The total play time when loaded this save
        /// </summary>
        [SaveProperty("mPlayDurationWhenLoaded")]
        public int PlayDurationWhenLoaded { get; set; }

        [SaveProperty("mReplicatedSessionName")]
        public string ReplicatedSessionName { get; set; }

        /// <summary>
        ///     Track if a hub part is needed for adding to player inventory when they respawn
        /// </summary>
        [SaveProperty("mForceAddHubPartOnSpawn")]
        public bool ForceAddHubPartOnSpawn { get; set; }

        [SaveProperty("mNumAdditionalArmEquipmentSlots")]
        public int NumAdditionalArmEquipmentSlots { get; set; }

        public List<ObjectReference> PlayerStates { get; } = new List<ObjectReference>();

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                PlayerStates.Add(reader.ReadObjectReference());
            }
        }
    }
}
