using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveObjectModel : ViewModelBase
    {
        private string title;
        private string rootObject;
        private string type;
        private bool isSelected;
        private bool isExpanded;

        public RelayCommand CopyNameCommand { get; }
        public RelayCommand CopyPathCommand { get; }
        public RelayCommand AddPropertyCommand { get; }
        public RelayCommand<SerializedPropertyViewModel> RemovePropertyCommand { get; }

        public ObservableCollection<SaveObjectModel> Items { get; } = new ObservableCollection<SaveObjectModel>();

        public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

        /// <summary>
        /// Recursively gets all the SaveObject children in the tree plus self
        /// </summary>
        public List<SaveObject> DescendantSelf
        {
            get
            {
                var list = new List<SaveObject>();
                if (Model != null) list.Add(Model);

                foreach (var item in Items) list.AddRange(item.DescendantSelf);

                return list;
            }
        }

        public string Title
        {
            get => title;
            set { Set(() => Title, ref title, value); }
        }

        public string RootObject
        {
            get => rootObject;
            set { Set(() => RootObject, ref rootObject, value); }
        }

        public string Type
        {
            get => type;
            set { Set(() => Type, ref type, value); }
        }

        public bool IsSelected
        {
            get => isSelected;
            set { Set(() => IsSelected, ref isSelected, value); }
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set { Set(() => IsExpanded, ref isExpanded, value); }
        }

        public SaveObject Model { get; }

        public SaveObjectModel(SaveObject model)
        {
            Model = model;
            Title = model.InstanceName;
            RootObject = model.RootObject;
            Type = model.TypePath.Split('/').Last();

            Fields = new ObservableCollection<SerializedPropertyViewModel>(Model.DataFields.Select(PropertyViewModelMapper.Convert));

            CopyNameCommand = new RelayCommand(CopyName);
            CopyPathCommand = new RelayCommand(CopyPath);
            AddPropertyCommand = new RelayCommand(AddProperty);
            RemovePropertyCommand = new RelayCommand<SerializedPropertyViewModel>(RemoveProperty);
        }

        public SaveObjectModel(string title)
        {
            Title = title;
            Type = title;
        }

        /// <summary>
        /// Recursively walks through the save tree and finds a child node with 'name' title
        /// Expand also opens all parent nodes on return
        /// </summary>
        /// <param name="name">Name of the searched node</param>
        /// <param name="expand">Whether the parents of searched node should be expanded</param>
        /// <returns></returns>
        public SaveObjectModel FindChild(string name, bool expand)
        {
            if (title == name)
            {
                if (expand) IsSelected = true;
                return this;
            }

            foreach (var item in Items)
            {
                var foundChild = item.FindChild(name, expand);
                if (foundChild != null)
                {
                    if (expand) IsExpanded = true;
                    return foundChild;
                }
            }

            return null;
        }

        public bool Remove(SaveObjectModel model)
        {
            if (this == model) throw new InvalidOperationException("Cannot remove root model");

            var found = Items.Remove(model);
            if (found) return true;

            foreach (var item in Items)
            {
                found = item.Remove(model);
                if (found) return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{Title} ({Items.Count})";
        }

        public virtual void ApplyChanges()
        {
            foreach (var item in Items)
            {
                item.ApplyChanges();
            }

            // This is because the named only (pink) nodes aren't actually a valid object in the game
            if (Model == null) return;

            Model.InstanceName = Title;
            Model.DataFields.Clear();
            foreach (var field in Fields)
            {
                field.ApplyChanges();
                Model.DataFields.Add(field.Model);
            }
        }

        private void AddProperty()
        {
            AddWindow window = new AddWindow
            {
                Owner = Application.Current.MainWindow
            };
            AddViewModel avm = (AddViewModel)window.DataContext;
            avm.ObjectModel = this;
            window.ShowDialog();
        }

        private void RemoveProperty(SerializedPropertyViewModel property)
        {
            Fields.Remove(property);
        }

        private void CopyName()
        {
            Clipboard.SetText(Title);
        }

        private void CopyPath()
        {
            Clipboard.SetText(Model.TypePath);
        }
    }
}
