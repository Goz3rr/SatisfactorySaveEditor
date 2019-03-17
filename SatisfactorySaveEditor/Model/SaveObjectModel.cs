using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.Model
{
    public class SaveObjectModel : ObservableObject
    {
        private string title;
        private string rootObject;
        private bool isSelected;
        private bool isExpanded;

        public ObservableCollection<SaveObjectModel> Items { get; } = new ObservableCollection<SaveObjectModel>();
        public ObservableCollection<SerializedProperty> Fields { get; }

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

        public SaveObjectModel(string title, SerializedFields fields, string rootObject)
        {
            Title = title;
            Fields = new ObservableCollection<SerializedProperty>(fields);
            RootObject = rootObject;
        }

        public SaveObjectModel(string title)
        {
            Title = title;
            Fields = new ObservableCollection<SerializedProperty>();
        }

        public SaveObjectModel FindChild(string name)
        {
            if (title == name)
            {
                IsSelected = true;
                return this;
            }

            foreach (var item in Items)
            {
                var foundChild = item.FindChild(name);
                if (foundChild != null)
                {
                    IsExpanded = true;
                    return foundChild;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return $"{Title} ({Items.Count})";
        }
    }
}
