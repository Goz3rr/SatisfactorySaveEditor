using System;

namespace SatisfactorySaveParser.Exceptions
{
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
