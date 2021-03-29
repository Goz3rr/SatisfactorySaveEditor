using NLog;
using SatisfactorySaveParser.Exceptions;
using System.IO;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Engine class: FSaveHeader
    ///     Header: FGSaveSystem.h
    /// </summary>
    public class FSaveHeader
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Save version number
        /// </summary>
        public SaveHeaderVersion HeaderVersion { get; set; }
        /// <summary>
        ///     Save build (feature) number
        /// </summary>
        public FSaveCustomVersion SaveVersion { get; set; }

        /// <summary>
        ///     Unknown magic int
        ///     Seems to always be 66297
        /// </summary>
        public int BuildVersion { get; set; }

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

        /// <summary>
        ///     The FEditorObjectVersion that this save file was written with
        /// </summary>
        public int EditorObjectVersion { get; set; }

        /// <summary>
        ///     Generic MetaData - Requested by Mods
        /// </summary>
        public string ModMetadata { get; set; }

        /// <summary>
        ///     Was this save ever saved with mods enabled?
        /// </summary>
        public bool IsModdedSave { get; set; }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((int)HeaderVersion);
            writer.Write((int)SaveVersion);
            writer.Write(BuildVersion);

            writer.WriteLengthPrefixedString(MapName);
            writer.WriteLengthPrefixedString(MapOptions);
            writer.WriteLengthPrefixedString(SessionName);

            writer.Write(PlayDuration);
            writer.Write(SaveDateTime);

            if (HeaderVersion >= SaveHeaderVersion.AddedSessionVisibility)
                writer.Write((byte)SessionVisibility);

            if (HeaderVersion >= SaveHeaderVersion.UE425EngineUpdate)
                writer.Write(EditorObjectVersion);

            if (HeaderVersion >= SaveHeaderVersion.AddedModdingParams)
            {
                writer.WriteLengthPrefixedString(ModMetadata);
                writer.Write(IsModdedSave ? 1 : 0);
            }
        }

        public static FSaveHeader Parse(BinaryReader reader)
        {
            var header = new FSaveHeader
            {
                HeaderVersion = (SaveHeaderVersion)reader.ReadInt32(),
                SaveVersion = (FSaveCustomVersion)reader.ReadInt32(),
                BuildVersion = reader.ReadInt32(),

                MapName = reader.ReadLengthPrefixedString(),
                MapOptions = reader.ReadLengthPrefixedString(),
                SessionName = reader.ReadLengthPrefixedString(),

                PlayDuration = reader.ReadInt32(),
                SaveDateTime = reader.ReadInt64()
            };

            log.Debug($"Read save header: HeaderVersion={header.HeaderVersion}, SaveVersion={(int)header.SaveVersion}, BuildVersion={header.BuildVersion}, MapName={header.MapName}, MapOpts={header.MapOptions}, Session={header.SessionName}, PlayTime={header.PlayDuration}, SaveTime={header.SaveDateTime}");

            if (header.HeaderVersion > SaveHeaderVersion.LatestVersion)
                throw new UnknownSaveVersionException(header.HeaderVersion);

            if (header.SaveVersion < FSaveCustomVersion.DROPPED_WireSpanFromConnnectionComponents || header.SaveVersion > FSaveCustomVersion.LatestVersion)
                throw new UnknownBuildVersionException(header.SaveVersion);

            if (header.HeaderVersion >= SaveHeaderVersion.AddedSessionVisibility)
            {
                header.SessionVisibility = (ESessionVisibility)reader.ReadByte();
                log.Debug($"SessionVisibility={header.SessionVisibility}");
            }

            if (header.HeaderVersion >= SaveHeaderVersion.UE425EngineUpdate)
            {
                header.EditorObjectVersion = reader.ReadInt32();
                log.Debug($"EditorObjectVersion={header.EditorObjectVersion}");
            }

            if (header.HeaderVersion >= SaveHeaderVersion.AddedModdingParams)
            {
                header.ModMetadata = reader.ReadLengthPrefixedString();
                header.IsModdedSave = reader.ReadInt32() > 0;
                log.Debug($"ModMetadata={header.ModMetadata}, IsModdedSave={header.IsModdedSave}");
            }

            return header;
        }
    }
}
