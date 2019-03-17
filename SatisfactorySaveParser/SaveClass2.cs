using System.IO;

namespace SatisfactorySaveParser
{
    public class SaveClass2 : SaveEntry
    {
        public string Str4 { get; set; }
        public int Int5 { get; set; }

        public SaveClass2(BinaryReader reader) : base(reader)
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
