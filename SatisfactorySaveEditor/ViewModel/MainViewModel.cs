using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using NLog;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Service;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.View;
using SatisfactorySaveParser.Save;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase, IDropTarget
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();
        private string _searchText;
        private string _jumpSearchText;
        private bool _isBusy = false;
        private bool _isJumping = false;
        private FGSaveSession _saveGame;
        private SaveObjectTreeModel _root;
        private string _saveFileName;
        private SaveObjectTreeModel _selectedItem;
        private SaveObjectTreeModel _lastMultiSelect;

        public ObservableCollection<SaveObjectTreeModel> RootItems { get; } = new ObservableCollection<SaveObjectTreeModel>();
        public ObservableCollection<string> LastFiles { get; } = new ObservableCollection<string>();

        public RelayCommand<SaveObjectTreeModel> TreeSelectCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand CheckUpdatesCommand { get; }
        public RelayCommand<CancelEventArgs> ExitCommand { get; }
        public RelayCommand<string> OpenBrowserCommand { get; }
        public RelayCommand PreferencesCommand { get; }
        public RelayCommand ResetSearchCommand { get; }
        public RelayCommand<SaveObjectTreeModel> DeleteCommand { get; }

        public RelayCommand<string> OpenCommand { get; }
        public RelayCommand<bool> SaveCommand { get; }
        public RelayCommand ManualBackupCommand { get; }
        public RelayCommand<string> JumpCommand { get; }
        public RelayCommand<string> JumpSearchCommand { get; }
        public RelayCommand JumpMenuCommand { get; }

        public SaveObjectTreeModel SelectedItem
        {
            get => _selectedItem;
            set 
            {
                Set(() => SelectedItem, ref _selectedItem, value);
                if (value != null) value.IsSelected = true;
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(() => IsBusy, ref _isBusy, value);
        }

        public bool IsJumping
        {
            get => _isJumping;
            set => Set(() => IsJumping, ref _isJumping, value);
        }

        public string FileName
        {
            get
            {
                if (_saveGame == null) return string.Empty;
                return string.Format(" - {1} [{0}]", _saveFileName, _saveGame.Header.SessionName);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(() => SearchText, ref _searchText, value);

                _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(() => Filter(value), _tokenSource.Token);
            }
        }

        public string JumpSearchText
        {
            get => _jumpSearchText;
            set => Set(() => JumpSearchText, ref _jumpSearchText, value);
        }

        public bool HasUnsavedChanges { get; set; }

        public IOProgressModel ProgressModel { get; } = new IOProgressModel();

        public MainViewModel()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1]))
            {
                Task.Run(() => Load(args[1]));
            }

            var savedFiles = Properties.Settings.Default.LastSaves?.Cast<string>().ToList();
            if (savedFiles != null)
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

            TreeSelectCommand = new RelayCommand<SaveObjectTreeModel>(SelectNode);
            AboutCommand = new RelayCommand(About);
            OpenBrowserCommand = new RelayCommand<string>(OpenBrowser);
            ExitCommand = new RelayCommand<CancelEventArgs>(Exit);
            DeleteCommand = new RelayCommand<SaveObjectTreeModel>(Delete, CanDelete);

            OpenCommand = new RelayCommand<string>((fileName) => Open(fileName));
            SaveCommand = new RelayCommand<bool>((saveAs) => Save(saveAs),(saveAs) => CanSave());
            ManualBackupCommand = new RelayCommand(() => CreateBackup(true), CanSave);
            ResetSearchCommand = new RelayCommand(ResetSearch);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            JumpSearchCommand = new RelayCommand<string>(JumpSearch);
            JumpMenuCommand = new RelayCommand(JumpMenu, CanSave);

            CheckUpdatesCommand = new RelayCommand(() =>
            {
                CheckForUpdate(true).ConfigureAwait(false);
            });
            PreferencesCommand = new RelayCommand(OpenPreferences);

            MessengerInstance.Register<NotificationMessage>(this, MultiSelectHandler);

            CheckForUpdate(false).ConfigureAwait(false);
        }

        private void MultiSelectHandler(NotificationMessage obj)
        {
            if (obj.Notification != "LastMultiSelect") return;
            if (!(obj.Sender is SaveObjectTreeModel node))
            {
                log.Error("Invalid sender for 'LastMultiSelect' notification. These can be only raised by SaveObjectTreeModel.");
                return;
            }

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                if (_lastMultiSelect != null && _lastMultiSelect != node)
                {
                    if (_lastMultiSelect.Parent == node.Parent)
                    {
                        var parent = node.Parent;
                        var startIndex = parent.Children.IndexOf(_lastMultiSelect);
                        var endIndex = parent.Children.IndexOf(node);

                        if (startIndex > endIndex)
                        {
                            var temp = startIndex;
                            startIndex = endIndex;
                            endIndex = temp;
                        }

                        for (int i = startIndex; i < endIndex; i++) parent.Children[i].MultiSelectChildren(true, false);
                    }
                }
            }

            _lastMultiSelect = node;
        }

        private void JumpMenu()
        {
            IsJumping = true;
            JumpSearchText = string.Empty;
        }

        /// <summary>
        /// Checks if it's possible to jump to the passed EntityName string
        /// </summary>
        /// <param name="target">The EntityName to jump to, in string format</param>
        /// <returns>True if rootItem contains the EntitiyName, false otherwise.</returns>
        private bool CanJump(string target) => _root.Children.Traverse(c => c.Children).Any(vm => vm.Name == target);

        /// <summary>
        /// Select the specified entity in the tree view
        /// </summary>
        /// <param name="target">EntityName of the entity to jump to</param>
        private void Jump(string target)
        {
            if (SelectedItem != null) SelectedItem.IsSelected = false;
            SelectedItem = _root.Children.Traverse(c => c.Children).First(vm => vm.Name == target);
        }

        /// <summary>
        /// Search for an entity in the tree view and select the first match if it exists
        /// </summary>
        /// <param name="target">EntityName of the entity to jump to</param>
        private void JumpSearch(string target)
        {
            IsJumping = false;

            if (SelectedItem != null) SelectedItem.IsSelected = false;
            SelectedItem = _root.Children.Traverse(c => c.Children).First(vm => vm.Name.ToLower().Contains(target.ToLower()));

            var currentOpened = SelectedItem;
            while (currentOpened != null)
            {
                currentOpened.IsExpanded = true;
                currentOpened = currentOpened.Parent;
            }
        }

        /// <summary>
        /// Checks if the passed model is not the rootItem of the save or a search result
        /// </summary>
        /// <param name="model"></param>
        /// <returns>If deletion is allowed</returns>
        private bool CanDelete(SaveObjectTreeModel model)
        {
            return model?.ObjectKind != SaveObjectTreeModel.SaveObjectTreeKind.BlankNode;
        }

        /// <summary>
        /// Removes the passed model from rootItem and raises property changed on the root item.
        /// </summary>
        /// <param name="model">The model to delete</param>
        private void Delete(SaveObjectTreeModel model)
        {
            model.Parent.Children.Remove(model);

            foreach (var child in model.Children.Traverse(m => m.Children)) _saveGame.RemoveObject(child.Model);
            _saveGame.RemoveObject(model.Model);
        }

        private void Filter(string value)
        {
            if (_saveGame == null) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    RootItems.Clear();
                    RootItems.Add(_root);
                });
            }
            else
            {
                var valueLower = value.ToLower(CultureInfo.InvariantCulture);
                var filter = _root.Children.Traverse(c => c.Children).WithCancellation(_tokenSource.Token).Where(vm => vm.Name.ToLower(CultureInfo.InvariantCulture).Contains(valueLower)).ToHashSet();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SaveObjectTreeModel searchResultNode;

                    if (!filter.Any()) searchResultNode = new SaveObjectTreeModel($"No search results found for '{value}'", null, true);
                    else
                    {
                        searchResultNode = new SaveObjectTreeModel($"Search result for '{value}'", null, true);
                        foreach (var item in filter) searchResultNode.Children.Add(item);
                    }

                    RootItems.Clear();
                    RootItems.Add(searchResultNode);
                });
            }
        }

        private void SelectNode(SaveObjectTreeModel node)
        {
            SelectedItem = node;
        }

        private void ResetSearch()
        {
            SearchText = null;
            SelectedItem = null;
        }

        private void OpenPreferences()
        {
            var window = new PreferencesWindow
            {
                Owner = Application.Current.MainWindow
            };

            window.ShowDialog();

            // We have to force a tree redraw in case the user changes the naming scheme
            foreach (var child in _root.Children.Traverse(c => c.Children)) child.RaisePropertyChanged(() => child.DisplayName);
        }

        /// <summary>
        /// Checks if there are unsaved changes, exits otherwise or if the user choses to discard.
        /// TODO: Mark as unsaved when property fileds are changed
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
        /// Save changes, creating a backup first if auto backups are enabled in the user's preferences
        /// </summary>
        /// <param name="saveAs">If the Save As... option box should be brought up to choose a destination</param>
        private void Save(bool saveAs)
        {
            if (saveAs)
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "Satisfactory save file|*.sav",
                    InitialDirectory = Path.GetDirectoryName(_saveFileName),
                    DefaultExt = ".sav",
                    CheckFileExists = false,
                    AddExtension = true
                };

                if (dialog.ShowDialog() == true)
                {
                    _saveFileName = dialog.FileName;

                    AddRecentFileEntry(dialog.FileName);
                    RaisePropertyChanged(() => FileName);
                }
                else return;
            }

            if (Properties.Settings.Default.AutoBackup)
            {
                Task.Run(() =>
                {
                    CreateBackup(false);
                    Save(_saveFileName);
                });
            }
            else Task.Run(() => Save(_saveFileName));

            HasUnsavedChanges = false;
        }

        private void CreateBackup(bool manual)
        {
            IsBusy = true;
            log.Info($"Creating a {(manual ? "manual " : "")}backup for {_saveFileName}");
            SaveIOService.CreateBackup(_saveFileName);

            if (manual) MessageBox.Show("Backup created. You can find it in your save file folder.", "Backup created");
            IsBusy = false;
        }

        /// <summary>
        /// Starts the process of loading a file, prompting the user if there are unsaved changes. Marks as having no unsaved changes
        /// </summary>
        /// <param name="fileName">Path to the save file</param>
        private void Open(string fileName)
        {
            if (HasUnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Abandon changes by opening another file?\n\nNote: Changes made in the data text fields are not yet tracked as saved/unsaved but are still saved.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No) return;
            }

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                Task.Run(() => Load(fileName));
                HasUnsavedChanges = false;
                return;
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
                Task.Run(() => Load(dialog.FileName));
                HasUnsavedChanges = false;
            }
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
        }

        /// <summary>
        /// Displays version information box
        /// </summary>
        private void About()
        {
            new AboutWindow
            {
                Owner = Application.Current.MainWindow
            }.ShowDialog();
        }

        /// <summary>
        /// Checks if the editor can perform a save operation
        /// </summary>
        /// <returns>True if a save game is currently loaded, false otherwise</returns>
        private bool CanSave()
        {
            return _saveGame != null;
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

        private void OpenBrowser(string url)
        {
            BrowserUtil.OpenBrowser(url);
        }

        private void Save(string fileName)
        {
            IsBusy = true;
            SaveIOService.Save(fileName, _saveGame);
            IsBusy = false;
        }

        private void Load(string fileName)
        {
            IsBusy = true;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("That file could no longer be found on the disk.\nIt has been removed from the recent files list.", "File not present", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (LastFiles != null && LastFiles.Contains(fileName)) //if the save file that failed to open was on the last found list, remove it. this should only occur when people move save files around and leave the editor open.
                {
                    log.Info($"Removing save file {fileName} from recent saves list since it wasn't found on disk");
                    Properties.Settings.Default.LastSaves.Remove(fileName);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastFiles.Remove(fileName);
                    });
                }
                return;
            }

            var (root, saveGame) = SaveIOService.Load(fileName, ProgressModel);

            Application.Current.Dispatcher.Invoke(() =>
            {
                RootItems.Clear();
                RootItems.Add(root);
            });

            _root = root;
            _saveGame = saveGame;
            _saveFileName = fileName;

            SelectedItem = _root;
            SearchText = null;

            RaisePropertyChanged(() => FileName);
            AddRecentFileEntry(fileName);

            IsBusy = false;
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
            Task.Run(() => Load(fileName));
        }
    }
}