using System;

namespace SatisfactorySaveParser.Exceptions
{
    public class UnknownSaveVersionException : Exception
    {
        public int SaveVersion { get; set; }

        public UnknownSaveVersionException(int saveVersion)
        {
            SaveVersion = saveVersion;
        }
    }
}
