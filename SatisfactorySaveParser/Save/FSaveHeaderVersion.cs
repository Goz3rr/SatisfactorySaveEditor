namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Enum taken from the game headers which is used to version the header
    /// </summary>
    public enum FSaveHeaderVersion
    {
        // First version
        InitialVersion = 0,

        // @2017-01-20: Added BuildVersion, MapName and MapOptions
        PrepareForLoadingMaps,

        // @2017-02-07: Added SessionId for autosaves
        AddedSessionId,

        // @2018-02-23 Added PlayDuration to header
        AddedPlayDuration,

        // @2018-04-10 SessionID from int32 to FString, also added when the save was saved
        SessionIDStringAndSaveTimeAdded,

        // @2019-01-15 Added session visibility to the header so we can set it up with the same visibility
        AddedSessionVisibility,

        // -----<new versions can be added above this line>-----
        VersionPlusOne,
        LatestVersion = VersionPlusOne - 1 // Last version to use
    }
}
