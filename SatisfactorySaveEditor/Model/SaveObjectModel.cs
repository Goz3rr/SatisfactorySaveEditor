using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveObjectModel : ObservableObject
    {
        private string title;
        private string rootObject;
        private bool isSelected;
        private bool isExpanded;

        public ObservableCollection<SaveObjectModel> Items { get; } = new ObservableCollection<SaveObjectModel>();

        public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

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

            Fields = new ObservableCollection<SerializedPropertyViewModel>(Model.DataFields.Select(PropertyViewModelMapper.Convert));
        }

        public SaveObjectModel(string title)
        {
            Title = title;
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

            Model.DataFields.Clear();
            foreach (var field in Fields)
            {
                field.ApplyChanges();
                Model.DataFields.Add(field.Model);
            }
        }
    }
}
