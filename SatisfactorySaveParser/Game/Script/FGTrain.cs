using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGTrain")]
    public class FGTrain : SaveActor
    {
        /// <summary>
        ///     Physics simulation for the train
        /// </summary>
        [SaveProperty("mSimulationData")]
        public FTrainSimulationData SimulationData { get; set; }

        /// <summary>
        ///     The name of this train.
        /// </summary>
        [SaveProperty("mTrainName")]
        public TextEntry TrainName { get; set; }

        /// <summary>
        ///     Train are a doubly linked list, use TTrainIterator to iterate over a train.
        /// </summary>
        [SaveProperty("FirstVehicle")]
        public ObjectReference FirstVehicle { get; set; }

        [SaveProperty("LastVehicle")]
        public ObjectReference LastVehicle { get; set; }

        /// <summary>
        ///     This trains time table.
        /// </summary>
        [SaveProperty("TimeTable")]
        public ObjectReference TimeTable { get; set; }

        /// <summary>
        ///     Is this train self driving
        /// </summary>
        [SaveProperty("mIsSelfDrivingEnabled")]
        public bool IsSelfDrivingEnabled { get; set; }
    }
}
