#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1819 // Properties should not return arrays

using System;
using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

// FGBuildableSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    /// <summary>
    ///     Subsystem responsible for spawning and maintaining buildables.
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_BuildableSubsystem.BP_BuildableSubsystem_C")]
    public class BuildableSubsystem : SaveActor
    {
        public const int BUILDABLE_COLORS_MAX_SLOTS = 16;

        /// <summary>
        ///     DEPRECATED - Use Linear Color instead
        /// </summary>
        [SaveProperty("mColorSlotsPrimary"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public FColor[] ColorSlotsPrimary { get; } = new FColor[BUILDABLE_COLORS_MAX_SLOTS];

        /// <summary>
        ///     DEPRECATED - Use Linear Color instead
        /// </summary>
        [SaveProperty("mColorSlotsSecondary"), Obsolete("Marked as deprecated in Satisfactory headers")]
        public FColor[] ColorSlotsSecondary { get; } = new FColor[BUILDABLE_COLORS_MAX_SLOTS];

        [SaveProperty("mColorSlotsPrimary_Linear")]
        public List<FLinearColor> ColorSlotsPrimary_Linear { get; } = new List<FLinearColor>();

        [SaveProperty("mColorSlotsSecondary_Linear")]
        public List<FLinearColor> ColorSlotsSecondary_Linear { get; } = new List<FLinearColor>();
    }
}
