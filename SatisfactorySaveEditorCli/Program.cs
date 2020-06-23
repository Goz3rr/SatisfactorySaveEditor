using System;
using System.IO;
using System.Linq;

using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveEditorCli
{
    class Program
    {
        private static readonly SatisfactorySaveSerializer serializer = new SatisfactorySaveSerializer();

        static void Main()
        {
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
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/new_save_format_preview.sav"))));
            //var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/FTLNews284X.sav"))));
            //var save = LoadSave("%USERPROFILE%/Downloads/satisfactory saves/Mega Base Update 3.sav");
            var save = LoadSave("%USERPROFILE%/Downloads/satisfactory saves/3MANSTANDING_2006-040101.sav");

            //DumpCompressedSave("%USERPROFILE%/Downloads/satisfactory saves/Coastal_City_6.sav");

            //TryAllSaves("%USERPROFILE%/Downloads/satisfactory saves/");
        }

        private static FGSaveSession LoadSave(string path)
        {
            return serializer.Deserialize(new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables(path))));
        }

        private static void DumpCompressedSave(string path)
        {
            var file = Environment.ExpandEnvironmentVariables(path);
            var buffer = serializer.DumpCompressedData(new MemoryStream(File.ReadAllBytes(file)));
            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".bin"), buffer.ToArray());
        }

        private static void TryAllSaves(string path)
        {
            foreach (var file in Directory.GetFiles(Environment.ExpandEnvironmentVariables(path), "*.sav", SearchOption.AllDirectories))
            {
                var save = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(file)));
            }
        }
    }
}
