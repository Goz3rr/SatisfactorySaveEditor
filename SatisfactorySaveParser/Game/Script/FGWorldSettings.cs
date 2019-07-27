using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGWorldSettings")]
    public class FGWorldSettings : SaveActor
    {
        [SaveProperty("mBuildableSubsystem")]
        public ObjectReference BuildableSubsystem { get; set; }

        [SaveProperty("mFoundationSubsystem")]
        public ObjectReference FoundationSubsystem { get; set; }
    }
}
