using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGTrain")]
    public class FGTrain : SaveActor
    {
        [SaveProperty("FirstVehicle")]
        public ObjectReference FirstVehicle { get; set; }

        [SaveProperty("LastVehicle")]
        public ObjectReference LastVehicle { get; set; }

        [SaveProperty("TimeTable")]
        public ObjectReference TimeTable { get; set; }
    }
}
