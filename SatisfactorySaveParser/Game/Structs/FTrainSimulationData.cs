namespace SatisfactorySaveParser.Game.Structs
{
    /// <summary>
    ///     The physics simulation data for the trains.
    /// </summary>
    [GameStruct("TrainSimulationData")]
    public class FTrainSimulationData : GameStruct
    {
        public override string StructName => "TrainSimulationData";

        [StructProperty("Velocity")]
        public float Velocity { get; set; }
    }
}
