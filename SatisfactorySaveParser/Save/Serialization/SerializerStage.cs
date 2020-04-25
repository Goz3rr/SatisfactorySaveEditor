namespace SatisfactorySaveParser.Save.Serialization
{
    public enum SerializerStage
    {
        // Loading
        FileOpen,
        ParseHeader,
        Decompressing,
        ReadObjects,
        ReadObjectData,
        ReadDestroyedObjects,
        BuildReferences,

        // Saving
        WriteHeader,
        WriteObjects,
        WriteObjectData,
        WriteDestroyedObjects,
        Compressing,
        FileWrite,

        Done,
    }
}
