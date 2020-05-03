using System.Collections.Generic;

using SatisfactorySaveParser.Save;

// FGRailroadSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    /// <summary>
    ///     Actor for handling the railroad network and the trains on it.
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_RailroadSubsystem.BP_RailroadSubsystem_C")]
    public class RailroadSubsystem : SaveActor
    {
        /// <summary>
        ///     All station identifiers in the world.
        /// </summary>
        [SaveProperty("mTrainStationIdentifiers")]
        public List<ObjectReference> TrainStationIdentifiers { get; } = new List<ObjectReference>();

        /// <summary>
        ///     All the trains in the world.
        /// </summary>
        [SaveProperty("mTrains")]
        public List<ObjectReference> Trains { get; } = new List<ObjectReference>();
    }
}
