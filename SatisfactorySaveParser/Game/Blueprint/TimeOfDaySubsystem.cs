using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TimeOfDaySubsystem.BP_TimeOfDaySubsystem_C")]
    public class TimeOfDaySubsystem : SaveActor
    {
        [SaveProperty("mDaySeconds")]
        public float DaySeconds { get; set; }

        [SaveProperty("mNumberOfPassedDays")]
        public int NumberOfPassedDays { get; set; }
    }
}
