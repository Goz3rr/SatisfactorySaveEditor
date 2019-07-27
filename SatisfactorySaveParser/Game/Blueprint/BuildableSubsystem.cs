using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_BuildableSubsystem.BP_BuildableSubsystem_C")]
    public class BuildableSubsystem : SaveActor
    {
        public const int BUILDABLE_COLORS_MAX_SLOTS = 16;

        [SaveProperty("mColorSlotsPrimary")]
        public FColor[] ColorSlotsPrimary { get; } = new FColor[BUILDABLE_COLORS_MAX_SLOTS];

        [SaveProperty("mColorSlotsSecondary")]
        public FColor[] ColorSlotsSecondary { get; } = new FColor[BUILDABLE_COLORS_MAX_SLOTS];
    }
}
