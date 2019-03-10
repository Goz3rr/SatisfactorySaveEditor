using System.IO;

namespace SatisfactorySaveParser.Entities
{
    public abstract class SaveEntity
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
        public int Int4 { get; set; }
        public byte[] Data5 { get; set; }
        public int Int6 { get; set; }
        public int Int7 { get; set; }
        public byte[] Data8 { get; set; }

        public abstract void ParseData(byte[] data);

        public override string ToString()
        {
            return Str1;
        }
    }
}
