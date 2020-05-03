using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

// FGFoundationSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    /// <summary>
    ///     Class keeping track of which buildables that for a building.
    /// </summary>
    [SaveObjectClass("/Script/FactoryGame.FGFoundationSubsystem")]
    public class FoundationSubsystem : SaveActor
    {
        /// <summary>
        ///     All the buildings in the game, map with foundation ID and the building struct.
        /// </summary>
        [SaveProperty("mBuildings")]
        public Dictionary<int, FBuilding> Buildings { get; } = new Dictionary<int, FBuilding>();
    }
}
