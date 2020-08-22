using System;
using System.Collections.Generic;
using System.IO;

using CommandLine;

using NLog;

using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveEditorCli
{
    public static class Program
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static readonly SatisfactorySaveSerializer gameSerializer = new SatisfactorySaveSerializer();
        private static readonly BinarySaveSerializer binarySerializer = new BinarySaveSerializer();
        private static readonly JsonSaveSerializer jsonSaveSerializer = new JsonSaveSerializer();

        private class Options
        {
            [Option('o', "output", HelpText = "File type to output")]
            public SaveFileType Output { get; set; }

            [Value(0, HelpText = "List of save files to open", Required = true)]
            public IEnumerable<string> Files { get; set; }
        }


        public static void Main(string[] args)
        {
            using var parser = new Parser(c =>
            {
                c.EnableDashDash = true;
                c.HelpWriter = Console.Error;
                c.AutoHelp = true;
                c.CaseInsensitiveEnumValues = true;
            });

            parser.ParseArguments<Options>(args).WithParsed(o =>
            {
                //foreach (var file in o.Files)
                //{
                //    var type = GetFileTypeFromExtension(file);
                //    var serializer = GetSerializeForType(type);

                //    using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                //    var save = serializer.Deserialize(stream);
                //}
            });


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
            //serializer.Serialize(save, File.OpenWrite(Environment.ExpandEnvironmentVariables("%USERPROFILE%/Downloads/satisfactory saves/3MANSTANDING_2006-040101-saved3.sav")));

            //DumpCompressedSave("%USERPROFILE%/Downloads/satisfactory saves/Coastal_City_6.sav");

            //TryAllSaves("%USERPROFILE%/Downloads/satisfactory saves/");
        }

        private static SaveFileType GetFileTypeFromExtension(string path)
        {
            return Path.GetExtension(path)?.ToUpperInvariant() switch
            {
                ".SAV" => SaveFileType.Game,
                ".BIN" => SaveFileType.Binary,
                ".JSON" => SaveFileType.Json,
                _ => throw new NotImplementedException(),
            };
        }

        private static ISaveSerializer GetSerializeForType(SaveFileType type)
        {
            return type switch
            {
                SaveFileType.Game => gameSerializer,
                SaveFileType.Binary => binarySerializer,
                SaveFileType.Json => jsonSaveSerializer,
                _ => throw new NotImplementedException(),
            };
        }

        private static FGSaveSession LoadSave(string path)
        {
            using var ms = new MemoryStream(File.ReadAllBytes(Environment.ExpandEnvironmentVariables(path)));
            return gameSerializer.Deserialize(ms);
        }

        private static void DumpCompressedSave(string path)
        {
            var file = Environment.ExpandEnvironmentVariables(path);
            using var ms = new MemoryStream(File.ReadAllBytes(file));
            var buffer = SatisfactorySaveSerializer.DumpCompressedData(ms);
            File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + ".bin"), buffer.ToArray());
        }

        private static void TryAllSaves(string path)
        {
            foreach (var file in Directory.GetFiles(Environment.ExpandEnvironmentVariables(path), "*.sav", SearchOption.AllDirectories))
            {
                using var ms = new MemoryStream(File.ReadAllBytes(file));
                var save = gameSerializer.Deserialize(ms);
            }
        }
    }
}
