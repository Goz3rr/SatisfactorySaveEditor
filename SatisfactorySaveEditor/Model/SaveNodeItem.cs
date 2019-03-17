using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.Model
{
    public class SaveNodeItem : ObservableObject
    {
        private string title;

        public ObservableCollection<SaveNodeItem> Items { get; } = new ObservableCollection<SaveNodeItem>();
        public ObservableCollection<SerializedProperty> Fields { get; }

        public string Title
        {
            get => title;
            set { Set(() => Title, ref title, value); }
        }

        public SaveNodeItem(string title, SerializedFields fields)
        {
            Title = title;
            Fields = new ObservableCollection<SerializedProperty>(fields.Fields);
        }

        public SaveNodeItem(string title)
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
