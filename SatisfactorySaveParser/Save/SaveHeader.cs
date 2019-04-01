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
            writer.Write(SaveDateTime);

            if(SaveVersion >= 5)
                writer.Write((byte)SessionVisibility);
        }

        public static SaveHeader Parse(BinaryReader reader)
        {
            var header = new SaveHeader
            {
                SaveVersion = reader.ReadInt32(),
                BuildVersion = reader.ReadInt32(),
                Magic = reader.ReadInt32(),

                MapName = reader.ReadLengthPrefixedString(),
                MapOptions = reader.ReadLengthPrefixedString(),
                SessionName = reader.ReadLengthPrefixedString(),

                PlayDuration = reader.ReadInt32(),
                SaveDateTime = reader.ReadInt64(),
            };

            if (header.SaveVersion >= 5)
                header.SessionVisibility = (ESessionVisibility)reader.ReadByte();

            return header;
        }
    }
}
