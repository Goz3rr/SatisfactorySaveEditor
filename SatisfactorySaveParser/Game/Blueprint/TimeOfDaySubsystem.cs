using SatisfactorySaveParser.Save;

// FGTimeSubsystem.h

namespace SatisfactorySaveParser.Game.Blueprint
{
    [SaveObjectClass("/Game/FactoryGame/-Shared/Blueprint/BP_TimeOfDaySubsystem.BP_TimeOfDaySubsystem_C")]
    public class TimeOfDaySubsystem : SaveActor
    {
        /// <summary>
        ///     How many seconds that has passed into our current day
        /// </summary>
        [SaveProperty("mDaySeconds")]
        public float DaySeconds { get; set; }

        /// <summary>
        ///     The current day that has passed
        /// </summary>
        [SaveProperty("mNumberOfPassedDays")]
        public int NumberOfPassedDays { get; set; }
    }
}
