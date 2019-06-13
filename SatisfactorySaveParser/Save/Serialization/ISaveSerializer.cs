using System.IO;

namespace SatisfactorySaveParser.Save.Serialization
{
    /// <summary>
    ///     The ISaveSerializer interface is used to expose the functionality required to (de)serialize a specific save format
    /// </summary>
    public interface ISaveSerializer
    {
        /// <summary>
        ///     Serialize the specific save out to the the specified stream
        /// </summary>
        /// <param name="save"></param>
        /// <param name="stream"></param>
        void Serialize(FGSaveSession save, Stream stream);

        /// <summary>
        ///     Deserialize a save from the specified stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        FGSaveSession Deserialize(Stream stream);
    }
}
