using System;
using System.IO;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var serializer = new SatisfactorySaveSerializer();
            var save = serializer.Deserialize(File.Open(Environment.ExpandEnvironmentVariables("%localappdata%/FactoryGame/Saved/SaveGames/NewerTestSave_160419-193440.sav"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

        }
    }
}
