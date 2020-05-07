using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGTrainPlatformConnection")]
    public class FGTrainPlatformConnection : SaveComponent
    {
        [SaveProperty("mComponentDirection")]
        public bool ComponentDirection { get; set; }

        [SaveProperty("platformOwner")]
        public ObjectReference PlatformOwner { get; set; }

        [SaveProperty("mRailroadTrackConnection")]
        public ObjectReference RailroadTrackConnection { get; set; }

        [SaveProperty("mConnectedTo")]
        public ObjectReference ConnectedTo { get; set; }
    }
}
