#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace SatisfactorySaveParser.Game.Enums
{
    public enum ETrainPlatformDockingStatus
    {
        ETPDS_None,
        ETPDS_WaitingToStart,
        ETPDS_Loading,
        ETPDS_Unloading,
        ETPDS_WaitingForTransfer,
        ETPDS_Complete
    }
}
