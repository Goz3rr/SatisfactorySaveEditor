using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Exception that indicates a generic fatal error occured while attempting to parse a save
    /// </summary>
    public class FatalSaveException : Exception
    {
        public FatalSaveException()
        {
        }

        public FatalSaveException(string message) : base(message)
        {
        }
    }
}