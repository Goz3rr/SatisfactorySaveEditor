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
using System.Linq;
using GongSolutions.Wpf.DragDrop;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.Data;

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase, IDropTarget
    {
        private SatisfactorySave saveGame;
        private SaveObjectModel rootItem;
        private SaveObjectModel selectedItem;

        public ObservableCollection<SaveObjectModel> RootItem => new ObservableCollection<SaveObjectModel> { rootItem };

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
                return string.Format(" - [{0}]", saveGame.FileName);
            }
        }

        public RelayCommand<SaveObjectModel> TreeSelectCommand { get; }
        public RelayCommand<string> JumpCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand<SaveObjectModel> DeleteCommand { get; }
        public RelayCommand<string> CheatCommand { get; }
        public RelayCommand<bool> SaveCommand { get; }

        public MainViewModel()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && File.Exists(args[1])) LoadFile(args[1]);

            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            ExitCommand = new RelayCommand(Exit);
            OpenCommand = new RelayCommand(Open);
            AboutCommand = new RelayCommand(About);
            DeleteCommand = new RelayCommand<SaveObjectModel>(Delete, CanDelete);
            SaveCommand = new RelayCommand<bool>(Save, CanSave);
            CheatCommand = new RelayCommand<string>(Cheat, CanCheat);
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

        private bool CanCheat(string target)
        {
            return rootItem != null;
        }

        private void Cheat(string cheatType)
        {
            switch (cheatType)
            {
                case "Research":
                    {
                        var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.schematicManager", false);
                        if (cheatObject == null)
                        {
                            MessageBox.Show("This save does not contain a schematicManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find schematicManager", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        foreach (var field in cheatObject.Fields)
                        {
                            if (field.PropertyName == "mAvailableSchematics" || field.PropertyName == "mPurchasedSchematics")
                            {
                                if (!(field is ArrayPropertyViewModel arrayField))
                                {
                                    MessageBox.Show("Expected schematic data is of wrong type.\nThis means that the loaded save is probably corrupt. Aborting.", "Wrong schematics type", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                foreach(var research in Researches.Values)
                                {
                                    if(!arrayField.Elements.Cast<ObjectPropertyViewModel>().Any(e => e.Str2 == research))
                                    {
                                        arrayField.Elements.Add(new ObjectPropertyViewModel(new ObjectProperty(null, "", research)));
                                    }
                                }
                            }
                        }

                        MessageBox.Show("All research successfully unlocked.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case "UnlockMap":
                    {
                        var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
                        if (cheatObject == null)
                        {
                            MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mIsMapUnlocked") is BoolPropertyViewModel mapUnlocked)
                        {
                            mapUnlocked.Value = true;
                        }
                        else
                        {
                            cheatObject.Fields.Add(new BoolPropertyViewModel(new BoolProperty("mIsMapUnlocked")
                            {
                                Value = true
                            }));
                        }

                        MessageBox.Show("Map unlocked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case "InventorySize":
                    {
                        var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
                        if (cheatObject == null)
                        {
                            MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        int oldSlots = 0;
                        int requestedSlots = 0;
                        if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntPropertyViewModel inventorySize)
                        {
                            oldSlots = inventorySize.Value;
                        }
                        
                        CheatInventoryWindow window = new CheatInventoryWindow(oldSlots)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        CheatInventoryViewModel cvm = (CheatInventoryViewModel)window.DataContext;
                        cvm.NumberChosen = oldSlots;
                        cvm.OldSlotsDisplay = oldSlots;
                        window.ShowDialog();
                        requestedSlots = cvm.NumberChosen;
                        

                        if (requestedSlots < 0 || requestedSlots == oldSlots) //TryParse didn't find a number, or cancel was clicked on the inputbox
                        {
                            MessageBox.Show("Bonus inventory slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else //TryParse found a number to use
                        {
                            if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntPropertyViewModel inventorySize2)
                            {
                                inventorySize2.Value = requestedSlots;
                            }
                            else
                            {
                                cheatObject.Fields.Add(new IntPropertyViewModel(new IntProperty("mNumAdditionalInventorySlots")
                                {
                                    Value = requestedSlots
                                }));
                            }

                            MessageBox.Show("Bonus inventory set to " + requestedSlots + " slots.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cheatType), cheatType, null);
            }
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
                    saveGame.Entries.Clear();
                    saveGame.Entries.AddRange(rootItem.DescendantSelf);

                    rootItem.ApplyChanges();
                    saveGame.Save(dialog.FileName);
                    RaisePropertyChanged(() => FileName);
                }
            }
            else
            {
                saveGame.Entries.Clear();
                saveGame.Entries.AddRange(rootItem.DescendantSelf);

                rootItem.ApplyChanges();
                saveGame.Save();
            }
        }

        private bool CanJump(string target)
        {
            return rootItem.FindChild(target, false) != null;
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
            SelectedItem = rootItem.FindChild(target, true);
        }

        private void SelectNode(SaveObjectModel node)
        {
            SelectedItem = node;
        }

        private void LoadFile(string path)
        {
            SelectedItem = null;

            saveGame = new SatisfactorySave(path);

            rootItem = new SaveObjectModel("Root");
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
            var fileName = ((DataObject) dropInfo.Data).GetFileDropList()[0];
            LoadFile(fileName);
        }
    }
}