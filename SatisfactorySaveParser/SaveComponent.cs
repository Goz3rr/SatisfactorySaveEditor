using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveComponent : SaveObject
    {
        public string Str4 { get; set; }
        public int Int5 { get; set; }

        public SaveComponent(BinaryReader reader) : base(reader)
        {
            Str4 = reader.ReadLengthPrefixedString();
            Int5 = reader.ReadInt32();
        }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
