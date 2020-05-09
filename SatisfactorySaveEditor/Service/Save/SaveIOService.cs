using NLog;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Service.Toast;
using SatisfactorySaveParser.Save;
using SatisfactorySaveParser.Save.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Service.Save
{
    public class SaveIOService
    {
        private readonly SatisfactorySaveSerializer _serializer = new SatisfactorySaveSerializer();
        private readonly ToastService _toastService;
        private readonly Logger log = LogManager.GetCurrentClassLogger();

        public ObservableCollection<string> LastFiles { get; } = new ObservableCollection<string>();

        public SaveIOService(ToastService toastService)
        {
            _toastService = toastService;

            var savedFiles = Properties.Settings.Default.LastSaves?.Cast<string>().Where(File.Exists).ToList() ?? new List<string>();
            LastFiles = new ObservableCollection<string>(savedFiles);
        }

        public void Cleanup()
        {
            Properties.Settings.Default.LastSaves.Clear();
            Properties.Settings.Default.LastSaves.AddRange(LastFiles.ToArray());
            Properties.Settings.Default.Save();
        }

        public (SaveObjectTreeModel root, SaveObjectTreeModel deletedRoot, FGSaveSession saveGame) Load(string fileName, IOProgressModel progressModel)
        {
            if (!File.Exists(fileName))
            {
                _toastService.Show("That file could no longer be found on the disk.\nIt has been removed from the recent files list.", "File not present", SystemIcons.Warning);

                if (LastFiles != null && LastFiles.Contains(fileName)) //if the save file that failed to open was on the last found list, remove it. this should only occur when people move save files around and leave the editor open.
                {
                    log.Info($"Removing save file {fileName} from recent saves list since it wasn't found on disk");
                    Application.Current.Dispatcher.Invoke(() => LastFiles.Remove(fileName));
                }

                return (null, null, null);
            }

            FGSaveSession saveGame;
            _serializer.DeserializationStageChanged += progressModel.UpdateStatusLoad;
            _serializer.DeserializationStageProgressed += progressModel.UpdateStatusLoad;

            try
            {
                using var file = File.Open(fileName, FileMode.Open);
                saveGame = _serializer.Deserialize(file);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the file:\n{ex.Message}\n\nCheck the logs for more details.\n\nIf this issue persists, please report it via \"Help > Report an Issue\", and attach the log file and save file you were trying to open.", "Error opening file", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error(ex);

                _serializer.DeserializationStageChanged -= progressModel.UpdateStatusLoad;
                _serializer.DeserializationStageProgressed -= progressModel.UpdateStatusLoad;
                return (null, null, null);
            }

            AddRecentFileEntry(fileName);
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

            var deletedRoot = new SaveObjectTreeModel("Deleted objects", null, true);
            foreach (var item in saveGame.DestroyedActors)
            {
                deletedRoot.Children.Add(new SaveObjectTreeModel(item, deletedRoot));
            }

            _serializer.DeserializationStageChanged -= progressModel.UpdateStatusLoad;
            _serializer.DeserializationStageProgressed -= progressModel.UpdateStatusLoad;
            return (root, deletedRoot, saveGame);
        }

        public void Save(string fileName, FGSaveSession saveGame)
        {
            using var file = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            AddRecentFileEntry(fileName);

            _serializer.Serialize(saveGame, file);
        }

        public void CreateBackup(string fileName)
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

        /// <summary>
        /// Adds a recently opened file to the list
        /// </summary>
        /// <param name="path">The path of the file to add</param>
        public void AddRecentFileEntry(string path)
        {
            if (LastFiles.Contains(path)) // No duplicates
            {
                Application.Current.Dispatcher.Invoke(() => LastFiles.Remove(path));
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                LastFiles.Add(path);

                // Keeps only 5 most recent saves
                while (Properties.Settings.Default.LastSaves.Count >= 6) LastFiles.RemoveAt(0);
            });
        }
    }
}
