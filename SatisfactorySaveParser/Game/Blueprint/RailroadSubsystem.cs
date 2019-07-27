using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_RailroadSubsystem.BP_RailroadSubsystem_C")]
    public class RailroadSubsystem : SaveActor
    {
        [SaveProperty("mTrainStationIdentifiers")]
        public List<ObjectReference> TrainStationIdentifiers { get; } = new List<ObjectReference>();

        [SaveProperty("mTrains")]
        public List<ObjectReference> Trains { get; } = new List<ObjectReference>();
    }
}
