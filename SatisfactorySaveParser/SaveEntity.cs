using System.IO;

namespace SatisfactorySaveParser
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
        
        public string DataStr1 { get; set; }
        public string DataStr2 { get; set; }
        public int DataInt3 { get; set; }
        public string DataStr4 { get; set; }
        public int DataInt5 { get; set; }


        public virtual void ParseData(uint length, BinaryReader reader)
        {
            DataStr1 = reader.ReadLengthPrefixedString();
            DataStr2 = reader.ReadLengthPrefixedString();
            DataInt3 = reader.ReadInt32();
            DataStr4 = reader.ReadLengthPrefixedString();
            DataInt5 = reader.ReadInt32();
        }

        public override string ToString()
        {
            return Str1;
        }
    }
}
