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
        public RelayCommand ExitCommand { get; }
        public RelayCommand<string> OpenCommand { get; }
        public RelayCommand Help_ViewGithubCommand { get; }
        public RelayCommand Help_ReportIssueCommand { get; }
        public RelayCommand Help_RequestHelpDiscordCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand<SaveObjectModel> DeleteCommand { get; }
        public RelayCommand<ICheat> CheatCommand { get; }
        public RelayCommand<bool> SaveCommand { get; }
        public RelayCommand ResetSearchCommand { get; }
        public RelayCommand CheckUpdatesCommand { get; }
        public RelayCommand PreferencesCommand { get; }

        public bool HasUnsavedChanges { get; set; } //TODO: set this to true when any value in WPF is changed. current plan for this according to goz3rr is to make a wrapper for the data from the parser and then change the set method in the wrapper

        public MainViewModel()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1])) LoadFile(args[1]);

            var savedFiles = Properties.Settings.Default.LastSaves?.Cast<string>().ToList();
            if (savedFiles == null) LastFiles = new ObservableCollection<string>();
            else LastFiles = new ObservableCollection<string>(savedFiles);

            // TODO: load this dynamically
            CheatMenuItems.Add(new ResearchUnlockCheat());
            CheatMenuItems.Add(new UnlockMapCheat());
            CheatMenuItems.Add(new InventorySlotsCheat());
            CheatMenuItems.Add(new KillPlayersCheat());
            CheatMenuItems.Add(new DeleteAnimalsCheat());

            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            ExitCommand = new RelayCommand(Exit);
            OpenCommand = new RelayCommand<string>(Open);
            AboutCommand = new RelayCommand(About);
            Help_ViewGithubCommand = new RelayCommand(Help_ViewGithub);
            Help_ReportIssueCommand = new RelayCommand(Help_ReportIssue);
            Help_RequestHelpDiscordCommand = new RelayCommand(Help_RequestHelpDiscord);
            CheckUpdatesCommand = new RelayCommand(() =>
            {
                CheckForUpdate(true).ConfigureAwait(false);
            });
            PreferencesCommand = new RelayCommand(OpenPreferences);

            DeleteCommand = new RelayCommand<SaveObjectModel>(Delete, CanDelete);
            SaveCommand = new RelayCommand<bool>(Save, CanSave);
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

        private bool CanDelete(SaveObjectModel model)
        {
            return model != rootItem;
        }

        private void Delete(SaveObjectModel model)
        {
            rootItem.Remove(model);
            RaisePropertyChanged(() => RootItem);
        }

        private bool CanCheat(ICheat cheat)
        {
            return rootItem != null;
        }

        private void Cheat(ICheat cheat)
        {
            if (cheat.Apply(rootItem))
                HasUnsavedChanges = true;
        }

        private bool CanSave(bool saveAs)
        {
            return saveGame != null;
        }

        private void Save(bool saveAs)
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
                    var newObjects = rootItem.DescendantSelf;
                    saveGame.Entries = saveGame.Entries.Intersect(newObjects).ToList();
                    saveGame.Entries.AddRange(newObjects.Except(saveGame.Entries));

                    rootItem.ApplyChanges();
                    saveGame.Save(dialog.FileName);
                    HasUnsavedChanges = false;
                    RaisePropertyChanged(() => FileName);
                    AddRecentFileEntry(dialog.FileName);
                }
            }
            else
            {
                var newObjects = rootItem.DescendantSelf;
                saveGame.Entries = saveGame.Entries.Intersect(newObjects).ToList();
                saveGame.Entries.AddRange(newObjects.Except(saveGame.Entries));

                rootItem.ApplyChanges();
                saveGame.Save();
                HasUnsavedChanges = false;
            }
        }

        private bool CanJump(string target)
        {
            return rootItem.FindChild(target, false) != null;
        }

        private void Help_ViewGithub()
        {
            System.Diagnostics.Process.Start("https://github.com/Goz3rr/SatisfactorySaveEditor#help");
        }

        private void Help_ReportIssue()
        {
            System.Diagnostics.Process.Start("https://github.com/Goz3rr/SatisfactorySaveEditor/issues");
        }

        private void Help_RequestHelpDiscord()
        {
            MessageBox.Show("You are now being redirected to the Satisfactory Modding discord server. Please request help in the #savegame-edits channel.");
            System.Diagnostics.Process.Start("https://discord.gg/rNxYXht"); //discord invite that links to the #savegame-edits channel
        }

        private void About()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show($"Satisfactory save editor{Environment.NewLine}{version}", "About");
        }

        private void Open(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                LoadFile(fileName);
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
                LoadFile(dialog.FileName);
                HasUnsavedChanges = false;
            }
        }

        private void Exit()
        {
            if (HasUnsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("You have unsaved changes. Close and abandon changes?\n\nNote: Changes made in the data text fields are not yet tracked as saved/unsaved but are still saved.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                    Application.Current.Shutdown();
                else
                    return;
            }
            else
            {
                Application.Current.Shutdown();
            }

        }

        private void Jump(string target)
        {
            SelectedItem.IsSelected = false;
            SelectedItem = rootItem.FindChild(target, true);
        }

        private void SelectNode(SaveObjectModel node)
        {
            SelectedItem = node;
        }

        private void LoadFile(string path)
        {
            SelectedItem = null;
            SearchText = null;

            saveGame = new SatisfactorySave(path);

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

        private void AddRecentFileEntry(string path)
        {
            if (Properties.Settings.Default.LastSaves == null)
            {
                Properties.Settings.Default.LastSaves = new StringCollection();
            }

            if (LastFiles.Contains(path)) // No duplicates
            {
                Properties.Settings.Default.LastSaves.Remove(path);
                LastFiles.Remove(path);
            }

            Properties.Settings.Default.LastSaves.Add(path);
            LastFiles.Add(path);

            while (Properties.Settings.Default.LastSaves.Count >= 6) // Keeps only 5 most recent saves
            {
                LastFiles.RemoveAt(0);
                Properties.Settings.Default.LastSaves.RemoveAt(0);
            }

            Properties.Settings.Default.Save();

            RootItem.Clear();
            RootItem.Add(rootItem);
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
                var filter = rootItem.DescendantSelfViewModel.WithCancellation(tokenSource.Token).Where(vm => vm.Model?.InstanceName.ToLower(CultureInfo.InvariantCulture).Contains(valueLower) ?? false);
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

        public void Drop(IDropInfo dropInfo)
        {
            var fileName = ((DataObject)dropInfo.Data).GetFileDropList()[0];
            LoadFile(fileName);
        }
    }
}