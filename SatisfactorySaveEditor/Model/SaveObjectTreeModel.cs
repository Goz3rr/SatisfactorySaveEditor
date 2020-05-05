using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.Save;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Model
{
    public class SaveObjectTreeModel : ObservableObject
    {
        public enum SaveObjectTreeKind
        {
            Component = 0,
            Actor = 1,
            TreeNode = 2, // This is actually not a real node, it exists only in the editor for organisation
            BlankNode = 3, // Same as TreeNode just without any prefix in tree
        }

        // These are static so WPF doesnt keep a copy for every single tree node, it ends up being like 200 MB of references on large saves
        public static readonly RelayCommand<SaveObjectTreeModel> CopyNameCommand = new RelayCommand<SaveObjectTreeModel>(CopyName);
        public static readonly RelayCommand<SaveObjectTreeModel> CopyPathCommand = new RelayCommand<SaveObjectTreeModel>(CopyPath);

        private bool _isExpanded;
        private bool _isSelected;

        public SaveObject Model { get; }

        public SaveObjectTreeModel Parent { get; }
        public ObservableCollection<SaveObjectTreeModel> Children { get; } = new ObservableCollection<SaveObjectTreeModel>();

        public string Name { get; }
        public SaveObjectTreeKind ObjectKind { get; }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(() => IsExpanded, ref _isExpanded, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(() => IsSelected, ref _isSelected, value);
        }

        public string DisplayName => ToString();

        public string Path
        {
            get 
            {
                if (ObjectKind == SaveObjectTreeKind.BlankNode) return string.Empty;
                if (Parent == null) return Name;
                else return string.Intern($"{Parent.Path}/{Name}");
            }
        }

        public SaveObjectTreeModel(SaveObject model, SaveObjectTreeModel parent = null)
        {
            Model = model;
            Parent = parent;
            ObjectKind = (SaveObjectTreeKind) model.ObjectKind;

            Name = model.Instance.PathName;

            Children.CollectionChanged += Children_CollectionChanged;
        }

        public SaveObjectTreeModel(string name, SaveObjectTreeModel parent = null, bool isBlank = false)
        {
            Name = string.Intern(name);
            Parent = parent;
            if (isBlank) ObjectKind = SaveObjectTreeKind.BlankNode;
            else ObjectKind = SaveObjectTreeKind.TreeNode;

            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => DisplayName);
        }

        public SaveObjectTreeModel AddChild(SaveObject child, ReadOnlySpan<string> currentPath)
        {
            if (currentPath.Length == 0)
            {
                var viewModel = new SaveObjectTreeModel(child, this);
                Children.Add(viewModel);
                return viewModel;
            }

            var nextParentName = currentPath[0];
            var nextParent = Children.FirstOrDefault(c => c.Name == nextParentName);
            if (nextParent == null)
            {
                nextParent = new SaveObjectTreeModel(nextParentName, this);
                Children.Add(nextParent);
            }

            return nextParent.AddChild(child, currentPath.Slice(1));
        }

        public override string ToString()
        {
            string name = Name;
            if (Properties.Settings.Default.ShortenNames) name = string.Intern(name.Split('.', StringSplitOptions.RemoveEmptyEntries).Last());
            if (Children.Count == 0) return name;

            var targetType = ObjectKind == SaveObjectTreeKind.Actor ? SaveObjectTreeKind.Component : SaveObjectTreeKind.Actor;
            var totalCount = Children.Traverse(obj => obj.Children).Count(obj => obj.ObjectKind == targetType);
            

            if (targetType == SaveObjectTreeKind.Actor)
            {
                return totalCount switch
                {
                    1 => $"{name} ({totalCount} object)",
                    _ => $"{name} ({totalCount} objects)",
                };
            }
            else
            {
                return totalCount switch
                {
                    1 => $"{name} ({totalCount} component)",
                    _ => $"{name} ({totalCount} components)",
                };
            }
        }

        private static void CopyName(SaveObjectTreeModel model)
        {
            Clipboard.SetDataObject(model.Name);
        }

        private static void CopyPath(SaveObjectTreeModel model)
        {
            Clipboard.SetDataObject(model.Path);
        }
    }
}
