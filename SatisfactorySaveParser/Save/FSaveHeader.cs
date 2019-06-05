using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     The save header holds metadata such as version numbers and session options of a save
    /// </summary>
    public class FSaveHeader
    {
        private ESessionVisibility sessionVisibility;

        /// <summary>
        ///     Header version number
        /// </summary>
        public FSaveHeaderVersion HeaderVersion { get; set; }

        /// <summary>
        ///     Save version number
        /// </summary>
        public FSaveCustomVersion SaveVersion { get; set; }

        /// <summary>
        ///     Save build number 
        ///     Should indicate the build of the game that generated this save, but is currently always 66297
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
        ///     The session visibility of the game
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
        ///     Helper property that indicates if this save header supports SessionVisibility
        /// </summary>
        public bool SupportsSessionVisibility => HeaderVersion >= FSaveHeaderVersion.AddedSessionVisibility;
    }
}
