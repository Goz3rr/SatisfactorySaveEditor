using System;
using System.IO;

using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveEditor
{
    class Program
    {
        static void Main()
        {
            var serializer = new SatisfactorySaveSerializer();
            //var save = serializer.Deserialize(File.Open(Environment.ExpandEnvironmentVariables("%localappdata%/FactoryGame/Saved/SaveGames/NewerTestSave_160419-193440.sav"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            //var save = serializer.Deserialize(File.Open(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/Main game.sav"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            //var save = serializer.Deserialize(File.Open(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/REAL_autosave_1.sav"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/REAL_autosave_1.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/New 24.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/PZ 102.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/space_war_090319-135233_-_Copy.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/new_autosave_1.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/Template Do Not Save Over ME.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/New Save - Copy (2).sav"))));
            var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/new_save_format_preview.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/FTLNews284X.sav"))));
        }
    }
}
