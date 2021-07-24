using System.IO;

namespace SatisfactorySaveParser.PropertyTypes.Structs
{
    public class DateTime : IStructData
    {
        public long Timestamp { get; set; }

        public int SerializedLength => 8;
        public string Type => "DateTime";

        public DateTime(BinaryReader reader)
        {
            Timestamp = reader.ReadInt64();
        }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            writer.Write(Timestamp);
        }
    }
}
