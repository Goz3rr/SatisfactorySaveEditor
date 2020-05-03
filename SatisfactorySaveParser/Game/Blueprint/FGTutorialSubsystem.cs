using System.Collections.Generic;

using SatisfactorySaveParser.Save;

// FGTutorialSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TutorialSubsystem.BP_TutorialSubsystem_C")]
    public class FGTutorialSubsystem : SaveComponent
    {
        /// <summary>
        ///     classes of things we have build
        /// </summary>
        [SaveProperty("mBuildingsBuilt")]
        public List<ObjectReference> BuildingsBuilt { get; } = new List<ObjectReference>();

        /// <summary>
        ///     Used to indicate if we should push the intro messages
        /// </summary>
        [SaveProperty("mHasSeenIntroTutorial")]
        public bool HasSeenIntroTutorial { get; set; }

        /// <summary>
        ///     Needed to set up delegates
        /// </summary>
        [SaveProperty("mOwningPlayerState")]
        public ObjectReference OwningPlayerState { get; set; }
    }
}
