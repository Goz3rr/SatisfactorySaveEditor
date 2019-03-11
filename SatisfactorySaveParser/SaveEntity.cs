using SatisfactorySaveParser.Fields;
using System.IO;

namespace SatisfactorySaveParser
{
    public abstract class SaveEntity
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }
        public int Int4 { get; set; }
        public byte[] Unknown5 { get; set; }
        public int Int6 { get; set; }
        public int Int7 { get; set; }
        
        public string DataStr1 { get; set; }
        public string DataStr2 { get; set; }
        public int DataInt3 { get; set; }
        public SerializedFields DataFields { get; set; }


        public virtual void ParseData(int length, BinaryReader reader)
        {
            var newLen = length - 12;
            DataStr1 = reader.ReadLengthPrefixedString();
            if (DataStr1.Length > 0)
                newLen -= DataStr1.Length + 1;

            DataStr2 = reader.ReadLengthPrefixedString();
            if (DataStr2.Length > 0)
                newLen -= DataStr2.Length + 1;

            DataInt3 = reader.ReadInt32();

            DataFields = SerializedFields.Parse(newLen, reader);
        }

        public override string ToString()
        {
            return Str1;
        }
    }
}
