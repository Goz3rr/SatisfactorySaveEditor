using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveComponent : SaveObject
    {
        /// <summary>
        ///     Instance name of the parent entity object
        /// </summary>
        public string ParentEntityName { get; set; }

        /// <summary>
        ///     Integer to keep track of amount of saved objects.
        ///     0 when followed by another object
        ///     Anything else is the total amount of objects in the save and indicated the end of the objects section
        /// </summary>
        public int SaveObjectCount { get; set; }

        public SaveComponent(BinaryReader reader) : base(reader)
        {
            ParentEntityName = reader.ReadLengthPrefixedString();
            SaveObjectCount = reader.ReadInt32();
        }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
