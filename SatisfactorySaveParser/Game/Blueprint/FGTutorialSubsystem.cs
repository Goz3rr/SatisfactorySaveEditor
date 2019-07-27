using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TutorialSubsystem.BP_TutorialSubsystem_C")]
    public class FGTutorialSubsystem : SaveComponent
    {
        [SaveProperty("mBuildingsBuilt")]
        public List<ObjectReference> BuildingsBuilt { get; } = new List<ObjectReference>();

        [SaveProperty("mHasSeenIntroTutorial")]
        public bool HasSeenIntroTutorial { get; set; }

        [SaveProperty("mOwningPlayerState")]
        public ObjectReference OwningPlayerState { get; set; }
    }
}
