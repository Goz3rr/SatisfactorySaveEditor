using System;

using SatisfactorySaveParser.Game.Enums;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     The save header holds metadata such as version numbers and session options of a save
    /// </summary>
    public class FSaveHeader
    {
        private ESessionVisibility sessionVisibility;
        private int editorObjectVersion;
        private string modMetadata;
        private bool isModdedSave;

        /// <summary>
        ///     Helper property that indicates if this save is compressed
        /// </summary>
        public bool IsCompressed => SaveVersion >= FSaveCustomVersion.SaveFileIsCompressed;

        /// <summary>
        ///     Header version number
        /// </summary>
        public FSaveHeaderVersion HeaderVersion { get; set; }

        /// <summary>
        ///     Save version number
        /// </summary>
        public FSaveCustomVersion SaveVersion { get; set; }

        /// <summary>
        ///     CL the game was on when this was stored
        ///     Hardcoded to 66297 for very old game versions
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

        /// <summary>
        ///     Helper property that indicates if this save header supports SessionVisibility
        /// </summary>
        public bool SupportsSessionVisibility => HeaderVersion >= FSaveHeaderVersion.AddedSessionVisibility;

        /// <summary>
        ///     The session visibility of the game.
        ///     Only valid for saves with HeaderVersion >= AddedSessionVisibility
        /// </summary>
        public ESessionVisibility SessionVisibility
        {
            get
            {
                if (!SupportsSessionVisibility)
                    throw new InvalidOperationException($"{nameof(SessionVisibility)} is not supported for this save version");

                return sessionVisibility;
            }
            set
            {
                if (!SupportsSessionVisibility)
                    throw new InvalidOperationException($"{nameof(SessionVisibility)} is not supported for this save version");

                sessionVisibility = value;
            }
        }

        /// <summary>
        ///     Helper property that indicates if this save header supports EditorObjectVersion
        /// </summary>
        public bool SupportsEditorObjectVersion => HeaderVersion >= FSaveHeaderVersion.UE425EngineUpdate;

        /// <summary>
        ///     Save the FEditorObjectVersion that this save file was written with
        ///     Only valid for saves with HeaderVersion >= UE425EngineUpdate
        /// </summary>
        public int EditorObjectVersion
        {
            get
            {
                if (!SupportsEditorObjectVersion)
                    throw new InvalidOperationException($"{nameof(EditorObjectVersion)} is not supported for this save version");

                return editorObjectVersion;
            }
            set
            {
                if (!SupportsEditorObjectVersion)
                    throw new InvalidOperationException($"{nameof(EditorObjectVersion)} is not supported for this save version");

                editorObjectVersion = value;
            }
        }

        /// <summary>
        ///     Helper property that indicates if this save header supports ModMetadata and IsModdedSave
        /// </summary>
        public bool SupportsModMetadata => HeaderVersion >= FSaveHeaderVersion.AddedModdingParams;

        /// <summary>
        ///     Generic MetaData - Requested by Mods
        ///     Only valid for saves with HeaderVersion >= AddedModdingParams
        /// </summary>
        public string ModMetadata
        {
            get
            {
                if (!SupportsModMetadata)
                    throw new InvalidOperationException($"{nameof(ModMetadata)} is not supported for this save version");

                return modMetadata;
            }
            set
            {
                if (!SupportsModMetadata)
                    throw new InvalidOperationException($"{nameof(ModMetadata)} is not supported for this save version");

                modMetadata = value;
            }
        }

        /// <summary>
        ///     Was this save ever saved with mods enabled?
        ///     Only valid for saves with HeaderVersion >= AddedModdingParams
        /// </summary>
        public bool IsModdedSave
        {
            get
            {
                if (!SupportsModMetadata)
                    throw new InvalidOperationException($"{nameof(IsModdedSave)} is not supported for this save version");

                return isModdedSave;
            }
            set
            {
                if (!SupportsModMetadata)
                    throw new InvalidOperationException($"{nameof(IsModdedSave)} is not supported for this save version");

                isModdedSave = value;
            }
        }
    }
}
