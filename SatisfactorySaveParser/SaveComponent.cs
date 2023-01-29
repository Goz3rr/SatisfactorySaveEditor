using NLog;
using System.IO;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     Engine class: FObjectSaveHeader
    /// </summary>
    public class SaveComponent : SaveObject
    {
        public const int TypeID = 0;

        /// <summary>
        ///     Instance name of the parent entity object
        /// </summary>
        public string ParentEntityName { get; set; }

        public SaveComponent(string levelname, string typePath, string rootObject, string instanceName) : base(levelname, typePath, rootObject, instanceName)
        {
        }

        public SaveComponent(string levelname, BinaryReader reader) : base(levelname, reader)
        {
            ParentEntityName = reader.ReadLengthPrefixedString();
        }

        public override void SerializeHeader(BinaryWriter writer)
        {
            base.SerializeHeader(writer);

            writer.WriteLengthPrefixedString(ParentEntityName);
        }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
