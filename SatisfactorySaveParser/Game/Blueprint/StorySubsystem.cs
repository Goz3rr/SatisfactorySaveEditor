using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

// FGStorySubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_StorySubsystem.BP_StorySubsystem_C")]
    public class StorySubsystem : SaveActor
    {
        /// <summary>
        ///     array of item descriptor class/message and if they have been found already
        /// </summary>
        [SaveProperty("mItemFoundData")]
        public List<FItemFoundData> ItemFoundData { get; } = new List<FItemFoundData>();
    }
}
