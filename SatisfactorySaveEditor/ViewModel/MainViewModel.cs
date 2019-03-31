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
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.Data;

namespace SatisfactorySaveEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
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
        public RelayCommand<object> AddPropertyCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand OpenCommand { get; }
        public RelayCommand AboutSaveCommand { get; }
        public RelayCommand AboutCommand { get; }
        public RelayCommand<SaveObjectModel> DeleteCommand { get; }
        public RelayCommand<string> CheatCommand { get; }
        public RelayCommand<bool> SaveCommand { get; }

        public bool HasUnsavedChanges { get; set; } //TODO: set this to true when any value in WPF is changed. current plan for this according to goz3rr is to make a wrapper for the data from the parser and then change the set method in the wrapper

        public MainViewModel()
        {
            TreeSelectCommand = new RelayCommand<SaveObjectModel>(SelectNode);
            JumpCommand = new RelayCommand<string>(Jump, CanJump);
            ExitCommand = new RelayCommand(Exit);
            OpenCommand = new RelayCommand(Open);
            AboutSaveCommand = new RelayCommand(AboutSave);
            AboutCommand = new RelayCommand(About);
            DeleteCommand = new RelayCommand<SaveObjectModel>(Delete, CanDelete);
            AddPropertyCommand = new RelayCommand<object>(AddProperty);
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
                        HasUnsavedChanges = true;
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
                        HasUnsavedChanges = true;
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
                        if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntProperty inventorySize)
                        {
                            oldSlots = inventorySize.Value;
                        }
                        
                        CheatInventoryWindow window = new CheatInventoryWindow(oldSlots)
                        {
                            Owner = Application.Current.MainWindow
                        };
                        CheatInventoryViewModel cvm = (CheatInventoryViewModel)window.DataContext;
                        //MessageBox.Show("old slots " + oldSlots);
                        cvm.NumberChosen = oldSlots;
                        cvm.OldSlotsDisplay = oldSlots;
                        window.ShowDialog();
                        requestedSlots = cvm.NumberChosen;
                        //MessageBox.Show("picked " + requestedSlots);
                        

                        if (requestedSlots < 0 || requestedSlots == oldSlots) //TryParse didn't find a number, or cancel was clicked on the inputbox
                        {
                            MessageBox.Show("Bonus inventory slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else //TryParse found a number to use
                        {
                            if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntProperty inventorySize2)
                            {
                                inventorySize2.Value = requestedSlots;
                            }
                            else
                            {
                                cheatObject.Fields.Add(new IntProperty("mNumAdditionalInventorySlots")
                                {
                                    Value = requestedSlots
                                });
                            }

                            HasUnsavedChanges = true;
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
                    rootItem.ApplyChanges();
                    saveGame.Save(dialog.FileName);
                    HasUnsavedChanges = false;
                    RaisePropertyChanged(() => FileName);
                }
            }
            else
            {
                rootItem.ApplyChanges();
                saveGame.Save();
                HasUnsavedChanges = false;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj));
            }
        }

        private bool CanJump(string target)
        {
            return rootItem.FindChild(target, false) != null;
        }

        private void AboutSave()
        {
            MessageBox.Show("TODO\n\nThe following wiki page may help.\n https://satisfactory.gamepedia.com/Save_File_Format", "About");
        }

        private void About()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            MessageBox.Show($"Satisfactory save editor{Environment.NewLine}{version}", "About");
        }

        private void Open()
        {
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
                Filter = "Satisfactory save file|*.sav",
                InitialDirectory = Environment.ExpandEnvironmentVariables(@"%userprofile%\Documents\My Games\FactoryGame\SaveGame\")
            };

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
    }
}