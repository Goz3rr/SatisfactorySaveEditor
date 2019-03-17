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

        public override string ToString()
        {
            return $"{Title} ({Items.Count})";
        }
    }
}
