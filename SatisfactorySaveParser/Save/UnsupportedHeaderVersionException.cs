using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Exception that indicates a save with unsupported header version (either too high or too low) was attempted to be loaded
    /// </summary>
    public class UnsupportedHeaderVersionException : Exception
    {
        /// <summary>
        ///     Header version of the save that was attempted to be loaded
        /// </summary>
        public FSaveHeaderVersion HeaderVersion { get; set; }

        public UnsupportedHeaderVersionException(FSaveHeaderVersion headerVersion)
        {
            HeaderVersion = headerVersion;
        }
    }
}
