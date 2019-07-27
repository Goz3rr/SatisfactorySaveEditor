using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    [SaveObjectClass("/Script/FactoryGame.FGTrainStationIdentifier")]
    public class FGTrainStationIdentifier : SaveActor
    {
        [SaveProperty("mStation")]
        public ObjectReference Station { get; set; }

        [SaveProperty("mStationName")]
        public TextEntry StationName { get; set; }
    }
}
