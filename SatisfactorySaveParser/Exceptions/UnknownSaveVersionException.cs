using SatisfactorySaveParser.Save;
using System;

namespace SatisfactorySaveParser.Exceptions
{
    public class UnknownSaveVersionException : Exception
    {
        public SaveHeaderVersion SaveVersion { get; set; }

        public UnknownSaveVersionException(SaveHeaderVersion saveVersion)
        {
            SaveVersion = saveVersion;
        }
    }
}
