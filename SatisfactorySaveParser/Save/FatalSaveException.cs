using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Exception that indicates a generic fatal error occured while attempting to parse a save
    /// </summary>
    public class FatalSaveException : Exception
    {
        /// <summary>
        ///     Position that was being read in the save before the last failed operation
        /// </summary>
        public long ReaderPosition { get; set; }

        public FatalSaveException()
        {
        }

        public FatalSaveException(string message) : base(message)
        {
        }

        public FatalSaveException(string message, long position) : base(message)
        {
            ReaderPosition = position;
        }

        public FatalSaveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}