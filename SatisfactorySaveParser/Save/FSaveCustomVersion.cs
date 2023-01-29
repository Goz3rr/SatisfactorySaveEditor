﻿namespace SatisfactorySaveParser.Save
{
    public enum FSaveCustomVersion
    {
        /** 2016/12/20 - Initial version of the save system, saves some character stuff like inventory and health */
        BeforeCustomVersionWasAdded = 0,

        /** 2017/01/23 - Now storing transform for all saved actors */
        DROPPED_StoreTransform,

        /** 2017/01/25 - Change object header, stops storing map name in savefile. Breaks all backward compability */
        DROPPED_ChangeObjectHeader,

        /** 2017/01/30 - Changed serialization of property names to strings instead of ints. Breaks all backward compability  */
        DROPPED_PropertyTagsAsStrings,

        /** 2017/01/31 - Now store the body state of all vehicles */
        DROPPED_StoreVehiclesBodyState,

        /** 2017/02/13 - Store if a actor was stored in the level in the actor TOC */
        DROPPED_ActorPlacedInLevelSaved,

        /** 2017/05/02 - Removed outer from save in actor header, now it's saved as a part of the actors property set */
        DROPPED_MovedActorOuter,

        /** 2017/05/23 - Rewrote power connections to use actor components. */
        DROPPED_PowerConnectionComponents,

        // 2017-10-09: Serialize train timetable
        DROPPED_SerializeTrainTimetable,

        // 2017-10-27: FactoryConnection transform converted from World to local
        DROPPED_FactoryConnectionWorldToLocal,

        // 2017-11-03: Circuits are now objects and not structs.
        DROPPED_CircuitObjects,

        // 2018-02-12: DockingStations now only have one inventory
        DROPPED_DockingStationSingleInventory,

        // 2018-03-06: Saving build shortcuts
        DROPPED_SavingBuildShortcuts,

        // 2018-04-05 Game Phase manager has been added and functionality has been moved from GameState.
        DROPPED_GamePhaseManagerAdded,

        // 2018-10-25 No longer save relative transforms for UFGConnectionComponent.
        DROPPED_RemovedRelativeTransformsFromConnectionComponents,

        // 2018-11-19 OnlineSubsystemMCP - Restore a lost pawn since we didn't have peoples EpicID
        DROPPED_MCP_RestoreLostPawn,

        // 2018-11-19 Wires no longer save locations, they span between connection components instead
        DROPPED_WireSpanFromConnnectionComponents,

        // 2019-01-30 Renamed SaveSessionId
        DROPPED_RenamedSaveSessionId,

        // 2019-06-20 GeoThermal generators didn't save resource nodes prior to this which results with issues when being dismantled
        ChangedGeoThermalGeneratorSaved,

        // 2019-06-24 NewRailroadSerialization to overwrite old railroad data
        OverwriteOldRailroadData,

        // 2019-07-24 Due to a bug in the network optimizations the legs data where trashed, reseting the legs to zero is the best option.
        ResetFactoryLegs,

        // 2019-08-28 The large portion of the save file is now compressed. The header is still intact at the start of the file but after that it is compressed with ZLIB.
        SaveFileIsCompressed,

        // 2020-02-09: This is so we can check if a save is older than BU3 and do certain migrations. 
        // For example save old number of inventory and arm slots to new system
        BU3SaveCompatibility,

        // 2020-03-19: Factory Colors have been converted to Linear Color. The buildable subsystem needs to convert from FColor to Linear Color overriding the defaults
        BuildingColorConversion,

        // 2020-04-21 Lizard doggos that had the friend status set but did not have a valid spawner saved will be recoupled with their spawner so that they are not killed bc of orphan status
        RescuedFriendDoggos,

        // 2020-06-02 Check if we have saved items in our inventory that is relevant to know if they are picked up
        CheckPickedUpItems,

        // 2021-02-05 Migrate Building Colros to FCustomUserColorData
        PerInstanceCustomColors,

        // 2021-09-14 Fix incorrect positioning of double ramp meshes. We then adjust position of double ramps in older saves in order to account for the change.
        DoubleRampPositioning,

        // 2021-09-21 Migrate FGTrain from native only to a blueprint class BP_Train.
        TrainBlueprintClassAdded,
        
        // 2021-12-03: Added sublevel streaming support
        AddedSublevelStreaming,
        
        // 2022-08-10: Added additional track progression path to resource sink subsystem
        AddedResourceSinkTrack,

        // 2022-07-28: Added Coloring support to concrete pillars, in post load we check if the swatch if the default one, if so we swap it with concrete.
        AddedColoringSupportToConcretePillars,

        // 2022-10-24: Readded since AddedColoringSupportToConcretePillars was merged to main
        AddedResourceSinkTrack2,

        // 2022-10-18: Added Cached locations for wire locations for use in visualization in blueprint hologram (can't depend on connection components)
        AddedCachedLocationsForWire,

        // 2022-11-17: Added migration of inventories from the old splitters and mergers to the new ones that have a smaller inventories.
        ReworkedSplittersAndMergers,

        // 2022-11-23: Added new productivity monitor implementation.
        ReworkedProductivityMonitor,

        // 2022-11-25: Nativized shopping list and added blueprint support.
        NativizedShoppingList,
        
        // -----<new versions can be added above this line>-------------------------------------------------
        VersionPlusOne,
        LatestVersion = VersionPlusOne - 1
    };
}
