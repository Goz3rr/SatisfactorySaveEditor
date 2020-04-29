using NLog;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Save.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;

namespace SatisfactorySaveEditor.Service
{
    // While not a real service I tried to separate as much IO and data manipulation from the actual viewmodel
    public static class SaveIOService
    {
        private static readonly SatisfactorySaveSerializer _serializer = new SatisfactorySaveSerializer();
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static (SaveObjectTreeModel root, FGSaveSession saveGame) Load(string fileName)
        {
            FGSaveSession saveGame;

            try
            {
                using var file = File.Open(fileName, FileMode.Open);
                saveGame = _serializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the file:\n{ex.Message}\n\nCheck the logs for more details.\n\nIf this issue persists, please report it via \"Help > Report an Issue\", and attach the log file and save file you were trying to open.", "Error opening file", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error(ex);
                return (null, null);
            }

            var root = new SaveObjectTreeModel(saveGame.Header.SessionName, null, true);

            var components = new HashSet<SaveComponent>();
            var actors = new Dictionary<string, SaveObjectTreeModel>();

            foreach (var item in saveGame.Objects)
            {
                // First we add only actors so we can attach components to them later
                if (item.ObjectKind == SaveObjectKind.Component)
                {
                    components.Add((SaveComponent)item);
                }
                else
                {
                    var splitPath = item.TypePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    var viewModel = root.AddChild(item, splitPath);
                    actors.Add(item.Instance.PathName, viewModel);
                }
            }

            foreach (var component in components)
            {
                var foundParentActor = actors.TryGetValue(component.ParentEntityName, out SaveObjectTreeModel actor);
                Trace.Assert(foundParentActor, "Found a component without a matching actor");

                var componentViewModel = new SaveObjectTreeModel(component, actor);
                actor.Children.Add(componentViewModel);
            }

            root.IsExpanded = true;
            foreach (var item in root.Children)
            {
                item.IsExpanded = true;
            }

            return (root, saveGame);
        }

        public static void Save(string fileName, FGSaveSession saveGame)
        {
            using var file = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            _serializer.Serialize(saveGame, file);
        }

        public static void CreateBackup(string fileName)
        {
            string saveFileDirectory = Path.GetDirectoryName(fileName);
            string tempDirectoryName = @"\SSEtemp\";
            string pathToZipFrom = saveFileDirectory + tempDirectoryName;

            string tempFilePath = saveFileDirectory + tempDirectoryName + Path.GetFileName(fileName);
            string backupFileFullPath = saveFileDirectory + @"\" + Path.GetFileNameWithoutExtension(fileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_SSEbackup.zip";

            try
            {
                //Satisfactory save files compress exceedingly well, so compress all backups so that they take up less space.
                //ZipFile only accepts directories, not single files, so copy the save to a temporary folder and then zip that folder
                Directory.CreateDirectory(pathToZipFrom);
                File.Copy(fileName, tempFilePath, true);
                ZipFile.CreateFromDirectory(pathToZipFrom, backupFileFullPath);
            }
            catch (Exception ex)
            {
                //should never be reached, but hopefully any users that encounter an error here will report it 
                MessageBox.Show("An error occurred while creating a backup. The error message will appear when you press 'Ok'.\nPlease tell Goz3rr, Robb, or virusek20 the contents of the error, or report it on the Github Issues page with your log file and save file attached.");
                log.Error(ex);
                throw;
            }
            finally
            {
                //delete the temporary folder and copy even if the zipping process fails
                File.Delete(tempFilePath);
                Directory.Delete(pathToZipFrom);
            }
        }
    }
}
