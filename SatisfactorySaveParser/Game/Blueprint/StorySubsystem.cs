using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_StorySubsystem.BP_StorySubsystem_C")]
    public class StorySubsystem : SaveActor
    {
        [SaveProperty("mItemFoundData")]
        public List<FItemFoundData> ItemFoundData { get; } = new List<FItemFoundData>();
    }
}
