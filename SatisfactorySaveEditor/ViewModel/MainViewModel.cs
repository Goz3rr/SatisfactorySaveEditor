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

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
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
        public RelayCommand ExitCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand AboutCommand { get; }

        public MainViewModel()
        {
            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump);
            ExitCommand = new RelayCommand(Exit);
            OpenCommand = new RelayCommand(Open);
            AboutCommand = new RelayCommand(About);

            LoadFile(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\space war_090319-135233 - Copy.sav");
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
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Jump(string target)
        {
            SelectedItem.IsSelected = false;
            SelectedItem = RootItem[0].FindChild(target);
        }

        private void SelectNode(SaveObjectModel node)
        {
            SelectedItem = node;
        }

        private void LoadFile(string path)
        {
            SelectedItem = null;

            var save = new SatisfactorySave(path);

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
                        items.Add(new SaveEntityModel(entry.InstanceName, entry.DataFields, entry.RootObject, se));
                        break;
                    case SaveComponent sc:
                        items.Add(new SaveComponentModel(entry.InstanceName, entry.DataFields, entry.RootObject, sc.ParentEntityName));
                        break;
                }
            }
        }
    }
}