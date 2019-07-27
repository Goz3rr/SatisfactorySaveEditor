using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Script/FactoryGame.FGFoundationSubsystem")]
    public class FoundationSubsystem : SaveActor
    {
        [SaveProperty("mBuildings")]
        public Dictionary<int, FBuilding> Buildings { get; } = new Dictionary<int, FBuilding>();
    }
}
