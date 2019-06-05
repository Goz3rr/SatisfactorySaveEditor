using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Exception that indicates a save with unsupported save version (either too high or too low) was attempted to be loaded
    /// </summary>
    public class UnsupportedSaveVersionException : Exception
    {
        /// <summary>
        ///     Save version of the save that was attempted to be loaded
        /// </summary>
        public FSaveCustomVersion SaveVersion { get; set; }

        public UnsupportedSaveVersionException(FSaveCustomVersion saveVersion)
        {
            SaveVersion = saveVersion;
        }
    }
}
