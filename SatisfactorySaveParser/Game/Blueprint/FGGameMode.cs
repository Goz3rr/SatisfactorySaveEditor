using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_GameMode.BP_GameMode_C")]
    public class FGGameMode : SaveActor
    {
        [SaveProperty("mLastAutosaveId")]
        public byte LastAutosaveId { get; set; }

        [SaveProperty("mSessionId")]
        public int SessionId_DEPRECATED { get; set; }

        [SaveProperty("mSessionIDString")]
        public string SessionIDString_DEPRECATED { get; set; }

        [SaveProperty("mSaveSessionName")]
        public string SaveSessionName { get; set; }

        [SaveProperty("mStartingPointTagName")]
        public string StartingPointTagName { get; set; }
    }
}
