using System;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Exceptions
{
    public class UnknownBuildVersionException : Exception
    {
        public FSaveCustomVersion BuildVersion { get; set; }

        public UnknownBuildVersionException(FSaveCustomVersion buildVersion)
        {
            BuildVersion = buildVersion;
        }
    }
}
