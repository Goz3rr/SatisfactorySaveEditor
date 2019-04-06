namespace SatisfactorySaveParser.Save
{
    public enum FSaveCustomVersion
    {
        BeforeCustomVersionWasAdded = 0,
        DROPPED_StoreTransform = 1,
        DROPPED_ChangeObjectHeader = 2,
        DROPPED_PropertyTagsAsStrings = 3,
        DROPPED_StoreVehiclesBodyState = 4,
        DROPPED_ActorPlacedInLevelSaved = 5,
        DROPPED_MovedActorOuter = 6,
        DROPPED_PowerConnectionComponents = 7,
        DROPPED_SerializeTrainTimetable = 8,
        DROPPED_FactoryConnectionWorldToLocal = 9,
        DROPPED_CircuitObjects = 10,
        DROPPED_DockingStationSingleInventory = 11,
        SavingBuildShortcuts = 12,
        DROPPED_GamePhaseManagerAdded = 13,
        RemovedRelativeTransformsFromConnectionComponents = 14,
        MCP_RestoreLostPawn = 15,
        WireSpanFromConnnectionComponents = 16,
        RenamedSaveSessionId = 17,
        VersionPlusOne = 18,
        LatestVersion = 17,
    };
}
