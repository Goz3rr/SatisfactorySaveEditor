using System;
using GalaSoft.MvvmLight;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using System.Windows;
using Microsoft.Win32;
using System.Reflection;
using SatisfactorySaveEditor.View;
using SatisfactorySaveParser.PropertyTypes;
using System.IO;

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _lastExportPath;

        private SaveObjectModel rootItem;
        private SaveObjectModel selectedItem;

        public ObservableCollection<SaveObjectModel> RootItem => new ObservableCollection<SaveObjectModel> { rootItem };

        public SaveObjectModel SelectedItem
        {
            get => selectedItem;
            set { Set(() => SelectedItem, ref selectedItem, value); }
        }

        public RelayCommand<SaveObjectModel> TreeSelectCommand { get; }
        public RelayCommand<string> JumpCommand { get; }
        public RelayCommand<object> AddPropertyCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand<bool> SaveCommand { get; }

        public MainViewModel()
        {
            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            ExitCommand = new RelayCommand(Exit);
            OpenCommand = new RelayCommand(Open);
            AboutCommand = new RelayCommand(About);
            AddPropertyCommand = new RelayCommand<object>(AddProperty);
            SaveCommand = new RelayCommand<bool>(Save);

#if DEBUG
            LoadFile(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\space war_090319-135233 - Copy.sav");
            _lastExportPath = @"%userprofile%\Documents\My Games\FactoryGame\SaveGame\space war_090319-135233 - Copy.sav";
#endif
        }

        private void Save(bool saveAs)
        {
            if (saveAs)
            {
                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = "Satisfactory save file|*.sav",
                    InitialDirectory = Path.GetDirectoryName(_lastExportPath),
                    FileName = Path.GetFileName(_lastExportPath),
                    DefaultExt = ".cpp",
                    CheckFileExists = false,
                    AddExtension = true
                };

                if (dialog.ShowDialog() == true)
                {
                    _lastExportPath = dialog.FileName;
                    // TODO: Here too
                }
            }
            else
            {
                // TODO: Save here
            }
        }

        private void AddProperty(object obj)
        {
            switch (obj)
            {
                case SaveObjectModel som:
                    AddWindow window = new AddWindow
                    {
                        Owner = Application.Current.MainWindow
                    };
                    AddViewModel avm = (AddViewModel)window.DataContext;
                    avm.ObjectModel = som;
                    window.ShowDialog();
                    break;
                case ArrayProperty ap:
                    ap.Elements.Add(AddViewModel.CreateProperty(AddViewModel.FromStringType(ap.Type), $"Element {ap.Elements.Count}"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj));
            }
        }

        private bool CanJump(string target)
        {
            return RootItem[0].FindChild(target, false) != null;
        }

        private void About()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show($"Satisfactory save editor{Environment.NewLine}{version}", "About");
        }

        private void Open()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Satisfactory save file|*.sav",
                InitialDirectory = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\")
            };

            if (dialog.ShowDialog() == true)
            {
                LoadFile(dialog.FileName);
                _lastExportPath = dialog.FileName;
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Jump(string target)
        {
            SelectedItem.IsSelected = false;
            SelectedItem = RootItem[0].FindChild(target, true);
        }

        private void SelectNode(SaveObjectModel node)
        {
            SelectedItem = node;
        }

        private void LoadFile(string path)
        {
            SelectedItem = null;

            var save = new SatisfactorySave(path);
#if DEBUG
            save.Save(path + ".saved");
#endif

            rootItem = new SaveObjectModel("Root");
            var saveTree = new EditorTreeNode("Root");

            foreach (var entry in save.Entries)
            {
                var parts = entry.TypePath.TrimStart('/').Split('/');
                saveTree.AddChild(parts, entry);
            }

            BuildNode(rootItem.Items, saveTree);

            RootItem[0].IsExpanded = true;
            foreach (var item in RootItem[0].Items)
            {
                item.IsExpanded = true;
            }

            RaisePropertyChanged(() => RootItem);
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
    }
}