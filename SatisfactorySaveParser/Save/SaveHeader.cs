using SatisfactorySaveParser.Exceptions;
using System.IO;

namespace SatisfactorySaveParser.Save
{
    public class SaveHeader
    {
        public const int ExpectedMagic = 66297;

        /// <summary>
        ///     Save version number
        /// </summary>
        public int SaveVersion { get; set; }
        /// <summary>
        ///     Save build (feature) number
        /// </summary>
        public FSaveCustomVersion BuildVersion { get; set; }

        /// <summary>
        ///     Unknown magic int
        ///     Seems to always be 66297
        /// </summary>
        public int Magic { get; set; }

        /// <summary>
        ///     The name of what appears to be the root object of the save.
        ///     Seems to always be "Persistent_Level"
        /// </summary>
        public string MapName { get; set; }
        /// <summary>
        ///     An URL style list of arguments of the session.
        ///     Contains the startloc, sessionName and Visibility
        /// </summary>
        public string MapOptions { get; set; }
        /// <summary>
        ///     Name of the saved game as entered when creating a new game
        /// </summary>
        public string SessionName { get; set; }

        /// <summary>
        ///     Amount of seconds spent in this save
        /// </summary>
        public int PlayDuration { get; set; }
        /// <summary>
        ///     Unix timestamp of when the save was saved
        /// </summary>
        public long SaveDateTime { get; set; }

        public ESessionVisibility SessionVisibility { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SaveVersion);
            writer.Write((int)BuildVersion);
            writer.Write(Magic);

            writer.WriteLengthPrefixedString(MapName);
            writer.WriteLengthPrefixedString(MapOptions);
            writer.WriteLengthPrefixedString(SessionName);

            writer.Write(PlayDuration);
            writer.Write(SaveDateTime);

            if (SaveVersion >= 5)
                writer.Write((byte)SessionVisibility);
        }

        public static SaveHeader Parse(BinaryReader reader)
        {
            var header = new SaveHeader
            {
                SaveVersion = reader.ReadInt32(),
                BuildVersion = (FSaveCustomVersion)reader.ReadInt32(),
                Magic = reader.ReadInt32(),

                MapName = reader.ReadLengthPrefixedString(),
                MapOptions = reader.ReadLengthPrefixedString(),
                SessionName = reader.ReadLengthPrefixedString(),

                PlayDuration = reader.ReadInt32(),
                SaveDateTime = reader.ReadInt64()
            };

            if (header.SaveVersion < 4 || header.SaveVersion > 5)
                throw new UnknownSaveVersionException(header.SaveVersion);

            if (header.BuildVersion < FSaveCustomVersion.WireSpanFromConnnectionComponents || header.BuildVersion > FSaveCustomVersion.LatestVersion)
                throw new UnknownBuildVersionException(header.BuildVersion);

            if (header.Magic != ExpectedMagic)
                throw new FatalSaveException($"Read magic header byte {header.Magic} but {ExpectedMagic} was expected");

            if (header.SaveVersion >= 5)
                header.SessionVisibility = (ESessionVisibility)reader.ReadByte();

            return header;
        }
    }
}
