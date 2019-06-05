using System.IO;

namespace SatisfactorySaveParser.Save.Serialization
{
    /// <summary>
    ///     The ISaveSerializer interface is used to expose the functionality required to (de)serialize a specific format
    /// </summary>
    public interface ISaveSerializer
    {
        void Serialize(FGSaveSession save, Stream fileStream);
        FGSaveSession Deserialize(Stream fileStream);
    }
}
