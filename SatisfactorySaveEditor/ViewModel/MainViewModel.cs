using System;
using GalaSoft.MvvmLight;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GongSolutions.Wpf.DragDrop;
using SatisfactorySaveEditor.Cheats;
using SatisfactorySaveEditor.View;
using System.IO.Compression;
using System.Windows.Threading;
using AsyncAwaitBestPractices.MVVM;
using NLog;
using System.ComponentModel;

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase, IDropTarget
    {
        private SatisfactorySave saveGame;
        private SaveObjectModel rootItem;
        private SaveObjectModel selectedItem;
        private string searchText;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private ObservableCollection<SaveObjectModel> rootItems = new ObservableCollection<SaveObjectModel>();
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private bool isBusyInternal = false;
        public bool IsBusy
        {
            get
            {
                return isBusyInternal;
            }
            set
            {
                Set(() => IsBusy, ref isBusyInternal, value);
            }
        }

        public ObservableCollection<SaveObjectModel> RootItem
        {
            get => rootItems;
            private set { Set(() => RootItem, ref rootItems, value); }
        }

        public SaveObjectModel SelectedItem
        {
            get => selectedItem;
            set { Set(() => SelectedItem, ref selectedItem, value); }
        }

        public string FileName
        {
            get
            {
                if (saveGame == null) return string.Empty;
                return string.Format(" - {1} [{0}]", saveGame.FileName, saveGame.Header.SessionName);
            }
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                Set(() => SearchText, ref searchText, value);

                tokenSource.Cancel();
                tokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() => Filter(value), tokenSource.Token);
            }
        }

        public ObservableCollection<string> LastFiles { get; } = new ObservableCollection<string>();

        public ObservableCollection<ICheat> CheatMenuItems { get; } = new ObservableCollection<ICheat>();

        public RelayCommand<SaveObjectModel> TreeSelectCommand { get; }
        public RelayCommand<string> JumpCommand { get; }
        public RelayCommand JumpMenuCommand { get; }
        public RelayCommand<CancelEventArgs> ExitCommand { get; }
        public RelayCommand<string> OpenCommand { get; }
        public RelayCommand Help_ViewGithubCommand { get; }
        public RelayCommand Help_ReportIssueCommand { get; }
        public RelayCommand Help_RequestHelpDiscordCommand { get; }
        public RelayCommand Help_FicsitAppGuideCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand<SaveObjectModel> DeleteCommand { get; }
        public RelayCommand<ICheat> CheatCommand { get; }
        public AsyncCommand<bool> SaveCommand { get; }
        public AsyncCommand ManualBackupCommand { get; }
        public RelayCommand ResetSearchCommand { get; }
        public RelayCommand CheckUpdatesCommand { get; }
        public RelayCommand PreferencesCommand { get; }

        public bool HasUnsavedChanges { get; set; } //TODO: set this to true when any value in WPF is changed. current plan for this according to goz3rr is to make a wrapper for the data from the parser and then change the set method in the wrapper

        public MainViewModel()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]))
            {
                var task = LoadFile(args[1]);
                task.Wait();
            }

            var savedFiles = Properties.Settings.Default.LastSaves?.Cast<string>().ToList();
            
            if(savedFiles != null)
            {
                bool modified = false;
                foreach (string filePath in savedFiles) //silently remove files that no longer exist from the list in Properties
                {
                    if (!File.Exists(filePath))
                    {
                        modified = true;
                        log.Info($"Removing save file {filePath} from recent saves list since it wasn't found on disk");
                        Properties.Settings.Default.LastSaves.Remove(filePath);
                    }
                }
                if (modified) //regenerate list since a save was not found when first built
                    savedFiles = Properties.Settings.Default.LastSaves?.Cast<string>().ToList();
                LastFiles = new ObservableCollection<string>(savedFiles);
            } 
            else //create a new empty collection for the list since there isn't anything there
                LastFiles = new ObservableCollection<string>();

            // TODO: load this dynamically
            CheatMenuItems.Add(new ResearchUnlockCheat());
            CheatMenuItems.Add(new UnlockMapCheat());
            CheatMenuItems.Add(new RevealMapCheat());
            CheatMenuItems.Add(new InventorySlotsCheat()); //inventory slot count works again (but is in a different place) as of Update 3
            CheatMenuItems.Add(new ArmSlotsCheat()); //inventory slot count works again (but is in a different place) as of Update 3
            CheatMenuItems.Add(new KillPlayersCheat());
            CheatMenuItems.Add(new CouponChangerCheat());
            DeleteEnemiesCheat deleteEnemiesCheat = new DeleteEnemiesCheat();
            CheatMenuItems.Add(deleteEnemiesCheat);
            CheatMenuItems.Add(new UndoDeleteEnemiesCheat());
            CheatMenuItems.Add(new SpawnDoggoCheat(deleteEnemiesCheat));
            CheatMenuItems.Add(new MassDismantleCheat());
            CheatMenuItems.Add(new EverythingBoxCheat());
            CheatMenuItems.Add(new CrateSummonCheat());
            CheatMenuItems.Add(new NoCostCheat());
            CheatMenuItems.Add(new NoPowerCheat());
            CheatMenuItems.Add(new RemoveSlugsCheat());
            CheatMenuItems.Add(new RestoreSlugsCheat());
            CheatMenuItems.Add(new DeduplicateSchematicsCheat());
            CheatMenuItems.Add(new U3DowngradeCheat());

            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            JumpMenuCommand = new RelayCommand(JumpMenu, () => CanSave(false)); //disallow menu jumping if no save is loaded
            ExitCommand = new RelayCommand<CancelEventArgs>(Exit);
            OpenCommand = new RelayCommand<string>(async (fileName) => await Open(fileName) );
            AboutCommand = new RelayCommand(About);
            Help_ViewGithubCommand = new RelayCommand(Help_ViewGithub);
            Help_ReportIssueCommand = new RelayCommand(Help_ReportIssue);
            Help_RequestHelpDiscordCommand = new RelayCommand(Help_RequestHelpDiscord);
            Help_FicsitAppGuideCommand = new RelayCommand(Help_FicsitAppGuide);
            CheckUpdatesCommand = new RelayCommand(() =>
            {
                CheckForUpdate(true).ConfigureAwait(false);
            });
            PreferencesCommand = new RelayCommand(OpenPreferences);

            DeleteCommand = new RelayCommand<SaveObjectModel>(Delete, CanDelete);
            SaveCommand = new AsyncCommand<bool>(async (saveAs) => await Save(saveAs), CanSave, null, true);
            ManualBackupCommand = new AsyncCommand(async () => await CreateBackup(true), CanSave);
            CheatCommand = new RelayCommand<ICheat>(Cheat, CanCheat);
            ResetSearchCommand = new RelayCommand(ResetSearch);

            CheckForUpdate(false).ConfigureAwait(false);
        }

        private void OpenPreferences()
        {
            var window = new PreferencesWindow
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();
        }

        private async Task CheckForUpdate(bool manual)
        {
            if (!manual && !Properties.Settings.Default.AutoUpdate) return;

            var latestVersion = await UpdateChecker.GetLatestReleaseInfo();

            if (latestVersion.IsNewer())
            {
                UpdateWindow window = new UpdateWindow
                {
                    DataContext = new UpdateWindowViewModel(latestVersion),
                    Owner = Application.Current.MainWindow
                };

                window.ShowDialog();
            }
            else if (manual)
            {
                MessageBox.Show("You are already using the latest version.", "Update", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Checks if the passed model is not the rootItem of the save
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If deletion is allowed</returns>
        private bool CanDelete(SaveObjectModel model)
        {
            return model != rootItem;
        }

        /// <summary>
        /// Removes the passed model from rootItem and raises property changed on the root item.
        /// </summary>
        /// <param name="model">The model to delete</param>
        private void Delete(SaveObjectModel model)
        {
            rootItem.Remove(model);
            RaisePropertyChanged(() => RootItem);
        }

        /// <summary>
        /// Checks if rootItem exists (if a save file is opened)
        /// </summary>
        /// <param name="cheat"></param>
        /// <returns>True if the root item is NOT null, false otherwise</returns>
        private bool CanCheat(ICheat cheat)
        {
            return rootItem != null;
        }

        /// <summary>
        /// Calls Apply() on the passed ICheat, providing it with rootItem. Only mark for unsaved changes if the cheat succeeds.
        /// </summary>
        /// <param name="cheat">The cheat to run</param>
        private void Cheat(ICheat cheat)
        {
            
            log.Info($"Applying cheat {cheat.Name}");
            if (cheat.Apply(rootItem, saveGame))
                HasUnsavedChanges = true;
        }

        /// <summary>
        /// Checks if the editor can perform a save operation
        /// </summary>
        /// <param name="saveAs">If the save operation is Save As (unused)</param>
        /// <returns>True if saveGame is NOT null, false otherwise</returns>
        private bool CanSave(object _)
        {
            return saveGame != null;
        }

        private bool CanSave() //overload of CanSave(bool saveAs) for contexts when saveAs doesn't matter
        {
            return CanSave(false);
        }

        /// <summary>
        /// Save changes, creating a backup first if auto backups are enabled in the user's preferences
        /// </summary>
        /// <param name="saveAs">(optional) If the Save As... option box should be brought up to choose a destination</param>
        private async Task Save(bool saveAs)
        {
            if (saveAs)
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "Satisfactory save file|*.sav",
                    InitialDirectory = Path.GetDirectoryName(saveGame.FileName),
                    DefaultExt = ".sav",
                    CheckFileExists = false,
                    AddExtension = true
                };

                if (dialog.ShowDialog() == true)
                {
                    await AutoBackupIfEnabled();

                    var newObjects = rootItem.DescendantSelf;
                    saveGame.Entries = saveGame.Entries.Intersect(newObjects).ToList();
                    saveGame.Entries.AddRange(newObjects.Except(saveGame.Entries));

                    rootItem.ApplyChanges();
                    this.IsBusy = true;
                    await Task.Run(() => saveGame.Save(dialog.FileName));
                    this.IsBusy = false;
                    HasUnsavedChanges = false;
                    RaisePropertyChanged(() => FileName);
                    AddRecentFileEntry(dialog.FileName);
                }
            }
            else
            {
                await AutoBackupIfEnabled();

                var newObjects = rootItem.DescendantSelf;
                saveGame.Entries = saveGame.Entries.Intersect(newObjects).ToList();
                saveGame.Entries.AddRange(newObjects.Except(saveGame.Entries));

                rootItem.ApplyChanges();
                this.IsBusy = true;
                await Task.Run(() => saveGame.Save());
                this.IsBusy = false;
                HasUnsavedChanges = false;
            }
        }

        private async Task AutoBackupIfEnabled()
        {
            if (Properties.Settings.Default.AutoBackup)
            {
                await CreateBackup(false);
            }
        }

        private async Task CreateBackup(bool manual)
        {
            this.IsBusy = true;
            await Task.Run(() => CreateBackupAsync(manual));
            this.IsBusy = false;
        }

        private void CreateBackupAsync(bool manual)
        {  
            string saveFileDirectory = Path.GetDirectoryName(saveGame.FileName);
            string tempDirectoryName = @"\SSEtemp\";
            string pathToZipFrom = saveFileDirectory + tempDirectoryName;

            string tempFilePath = saveFileDirectory + tempDirectoryName + Path.GetFileName(saveGame.FileName);
            string backupFileFullPath = saveFileDirectory + @"\" + Path.GetFileNameWithoutExtension(saveGame.FileName) + "_" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ".SSEbkup.zip";

            log.Info($"Creating a {(manual ? "manual " : "")}backup for {saveGame.FileName}");

            try
            {
                //Satisfactory save files compress exceedingly well, so compress all backups so that they take up less space.
                //ZipFile only accepts directories, not single files, so copy the save to a temporary folder and then zip that folder
                Directory.CreateDirectory(pathToZipFrom);
                File.Copy(saveGame.FileName, tempFilePath, true); 
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

            if (manual)
                MessageBox.Show("Backup created. Find it in your save file folder.");
        }

        /// <summary>
        /// Checks if it's possible to jump to the passed EntityName string
        /// </summary>
        /// <param name="target">The EntityName to jump to, in string format</param>
        /// <returns>True if rootItem contains the EntitiyName, false otherwise.</returns>
        private bool CanJump(string target)
        {
            return rootItem.FindChild(target, false) != null;
        }

        /// <summary>
        /// Opens the Github repo page scrolled to the 'Help' heading
        /// </summary>
        private void Help_ViewGithub()
        {
            System.Diagnostics.Process.Start("https://github.com/Goz3rr/SatisfactorySaveEditor#help");
        }

        /// <summary>
        /// Opens the Github repo page scrolled to the Issues tab
        /// </summary>
        private void Help_ReportIssue()
        {
            System.Diagnostics.Process.Start("https://github.com/Goz3rr/SatisfactorySaveEditor/issues");
        }

        /// <summary>
        /// Notifies the user of their redirection to the discord, then opens the invite.
        /// </summary>
        private void Help_RequestHelpDiscord()
        {
            MessageBox.Show("You are now being redirected to the Satisfactory Modding discord server. Please request help in the #savegame-edits channel.");
            System.Diagnostics.Process.Start("https://bit.ly/SatisfactoryModding"); //discord invite for Satisfactory Modding server. Contact BaineGames#7333 if it breaks
        }

        /// <summary>
        /// Notifies the user of their redirection to the ficsit.app guide, then opens the guide.
        /// </summary>
        private void Help_FicsitAppGuide()
        {
            MessageBox.Show("You are now being redirected to the ficsit.app mod and tool repository to view a guide.");
            System.Diagnostics.Process.Start("https://ficsit.app/guide/Z8h6z2CczH43c");
        }


        /// <summary>
        /// Displays version information box
        /// </summary>
        private void About()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show($"Satisfactory save editor{Environment.NewLine}{version}", "About");
        }

        /// <summary>
        /// Starts the process of loading a file, prompting the user if there are unsaved changes. Marks as having no unsaved changes
        /// </summary>
        /// <param name="fileName">Path to the save file</param>
        private async Task Open(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                await LoadFile(fileName);
                HasUnsavedChanges = false;

                return;
            }

            if (HasUnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Abandon changes by opening another file?\n\nNote: Changes made in the data text fields are not yet tracked as saved/unsaved but are still saved.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            //TODO: swap this over to calling the save method instead
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Satisfactory save file|*.sav"
            };

            var newPath = Environment.ExpandEnvironmentVariables(@"%localappdata%\FactoryGame\Saved\SaveGames\");
            var oldPath = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\");

            if (Directory.Exists(newPath)) dialog.InitialDirectory = newPath;
            else dialog.InitialDirectory = oldPath;

            if (dialog.ShowDialog() == true)
            {
                await LoadFile(dialog.FileName);
                HasUnsavedChanges = false;
            }
        }

        /// <summary>
        /// Checks if there are unsaved changes, exits otherwise or if the user choses to discard.
        /// TODO: Mark as unsaved when property fileds are changed
        /// TODO: Check this when pressing alt+f4 and clicking the red x
        /// </summary>
        private void Exit(CancelEventArgs args = null)
        {
            if (HasUnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Close and abandon changes?\n\nNote: Changes made in the data text fields are not yet tracked as saved/unsaved but are still saved.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    if (args != null) args.Cancel = true;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }

        }

        /// <summary>
        /// Select the specified entity in the tree view
        /// </summary>
        /// <param name="target">EntityName of the entity to jump to</param>
        private void Jump(string target)
        {
            if(SelectedItem != null)
                SelectedItem.IsSelected = false;
            SelectedItem = rootItem.FindChild(target, true);
        }

        /// <summary>
        /// Opens a StringPromptWindow prompting for an EntityName to jump to
        /// </summary>
        private void JumpMenu()
        {
            string destination = "";

            var dialog = new StringPromptWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (StringPromptViewModel)dialog.DataContext;
            cvm.WindowTitle = "Jump to Tag";
            cvm.PromptMessage = "Tag name:";
            cvm.ValueChosen = "";
            cvm.OldValueMessage = "Obtain via Right Click > Copy name\nExample:\nPersistent_Level:PersistentLevel.Char_Player_C_0.inventory";
            dialog.ShowDialog();

            destination = cvm.ValueChosen;

            if(!(destination.Equals("") || destination.Equals("cancel")))
                if (CanJump(destination))
                    Jump(destination);
                else
                    MessageBox.Show("Failed to jump to tag:\n" + destination);
        }

        /// <summary>
        /// Selects a node
        /// </summary>
        /// <param name="node">The node to select</param>
        private void SelectNode(SaveObjectModel node)
        {
            SelectedItem = node;
        }

        /// <summary>
        /// Loads a file into the editor
        /// </summary>
        /// <param name="path">The path to the file to open</param>
        private async Task LoadFile(string path)
        {
            SelectedItem = null;
            SearchText = null;

            this.IsBusy = true;
            await Task.Run(() => LoadFileAsync(path));
            this.IsBusy = false;
        }

        private void LoadFileAsync(string path)
        {
            try
            {
                saveGame = new SatisfactorySave(path);
            }
            catch (FileNotFoundException)
            {
                if (LastFiles != null && LastFiles.Contains(path)) //if the save file that failed to open was on the last found list, remove it. this should only occur when people move save files around and leave the editor open.
                {
                    MessageBox.Show("That file could no longer be found on the disk.\nIt has been removed from the recent files list.", "File not present", MessageBoxButton.OK, MessageBoxImage.Warning);
                    log.Info($"Removing save file {path} from recent saves list since it wasn't found on disk");
                    Properties.Settings.Default.LastSaves.Remove(path);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastFiles.Remove(path);
                    });
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the file:\n{ex.Message}\n\nCheck the logs for more details.\n\nIf this issue persists, please report it via \"Help > Report an Issue\", and attach the log file and save file you were trying to open.", "Error opening file", MessageBoxButton.OK, MessageBoxImage.Error);
                log.Error(ex);
                return;
            }
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                //manually raise these for the AsyncCommand library to pick up on it (ask virusek20 or Robb)
                SaveCommand.RaiseCanExecuteChanged();
                ManualBackupCommand.RaiseCanExecuteChanged();
            });
            
            rootItem = new SaveRootModel(saveGame.Header);
            var saveTree = new EditorTreeNode("Root");

            foreach (var entry in saveGame.Entries)
            {
                var parts = entry.TypePath.TrimStart('/').Split('/');
                saveTree.AddChild(parts, entry);
            }

            BuildNode(rootItem.Items, saveTree);

            rootItem.IsExpanded = true;
            foreach (var item in rootItem.Items)
            {
                item.IsExpanded = true;
            }

            RaisePropertyChanged(() => RootItem);
            RaisePropertyChanged(() => FileName);

            AddRecentFileEntry(path);
        }

        /// <summary>
        /// Adds a recently opened file to the list
        /// </summary>
        /// <param name="path">The path of the file to add</param>
        private void AddRecentFileEntry(string path)
        {
            if (Properties.Settings.Default.LastSaves == null)
            {
                Properties.Settings.Default.LastSaves = new StringCollection();
            }

            if (LastFiles.Contains(path)) // No duplicates
            {
                Properties.Settings.Default.LastSaves.Remove(path);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    LastFiles.Remove(path);
                });
                
            }

            Properties.Settings.Default.LastSaves.Add(path);
            Application.Current.Dispatcher.Invoke(() =>
            {
                LastFiles.Add(path);

                while (Properties.Settings.Default.LastSaves.Count >= 6) // Keeps only 5 most recent saves
                {
                    LastFiles.RemoveAt(0);
                    Properties.Settings.Default.LastSaves.RemoveAt(0);
                }
            });

            Properties.Settings.Default.Save();

            Application.Current.Dispatcher.Invoke(() =>
            {
                RootItem.Clear();
                RootItem.Add(rootItem);
            });
        }

        private void BuildNode(ObservableCollection<SaveObjectModel> items, EditorTreeNode node)
        {
            foreach (var child in node.Children)
            {
                var childItem = new SaveObjectModel(child.Value.Name);
                BuildNode(childItem.Items, child.Value);
                items.Add(childItem);
            }

            foreach (var entry in node.Content)
            {
                switch (entry)
                {
                    case SaveEntity se:
                        items.Add(new SaveEntityModel(se));
                        break;
                    case SaveComponent sc:
                        items.Add(new SaveComponentModel(sc));
                        break;
                }
            }
        }

        private void Filter(string value)
        {
            if (rootItem == null) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RootItem.Clear();
                    RootItem.Add(rootItem);
                });
            }
            else
            {
                var valueLower = value.ToLower(CultureInfo.InvariantCulture);
                var filter = rootItem.DescendantSelfViewModel.WithCancellation(tokenSource.Token).Where(vm => vm.MatchesFilter(valueLower));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RootItem = new ObservableCollection<SaveObjectModel>(filter);
                });
            }
        }

        private void ResetSearch()
        {
            SearchText = null;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo.Data is DataObject data)) return;

            var files = data.GetFileDropList();
            if (files == null || files.Count == 0) return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        /// <summary>
        /// Handle drag and drop opening of save files
        /// </summary>
        /// <param name="dropInfo"></param>
        public void Drop(IDropInfo dropInfo)
        {
            var fileName = ((DataObject)dropInfo.Data).GetFileDropList()[0];
            _ = LoadFile(fileName);
            // No need to wait for it to finish, since that blocks the dispatcher thread and causes a deadlock
        }
    }
}