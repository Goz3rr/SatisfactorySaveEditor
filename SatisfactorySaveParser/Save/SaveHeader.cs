using System.IO;

namespace SatisfactorySaveParser.Save
{
    public class SaveHeader
    {
        public int SaveVersion { get; set; }
        public int BuildVersion { get; set; }

        public int Magic { get; set; }
        
        public string MapName { get; set; }
        public string MapOptions { get; set; }
        public string SessionName { get; set; }

        public int PlayDuration { get; set; }

        public int Padding_0 { get; set; }
        public long SaveDateTime { get; set; }

        public ESessionVisibility SessionVisibility { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SaveVersion);
            writer.Write(BuildVersion);
            writer.Write(Magic);

            writer.WriteLengthPrefixedString(MapName);
            writer.WriteLengthPrefixedString(MapOptions);
            writer.WriteLengthPrefixedString(SessionName);

            writer.Write(PlayDuration);
            writer.Write(Padding_0);
            writer.Write(SaveDateTime);
            writer.Write((byte)SessionVisibility);
        }

        public static SaveHeader Parse(BinaryReader reader)
        {
            return new SaveHeader
            {
                SaveVersion = reader.ReadInt32(),
                BuildVersion = reader.ReadInt32(),
                Magic = reader.ReadInt32(),

                MapName = reader.ReadLengthPrefixedString(),
                MapOptions = reader.ReadLengthPrefixedString(),
                SessionName = reader.ReadLengthPrefixedString(),

                PlayDuration = reader.ReadInt32(),
                Padding_0 = reader.ReadInt32(),
                SaveDateTime = reader.ReadInt64(),
                SessionVisibility = (ESessionVisibility)reader.ReadByte()
            };
        }
    }
}
