using System.Collections.ObjectModel;

namespace SatisfactorySaveEditor
{
    public class SaveNodeItem
    {
        public ObservableCollection<SaveNodeItem> Items { get; set; } = new ObservableCollection<SaveNodeItem>();

        public string Title { get; set; }

        public SaveNodeItem(string title)
        {
            Title = title;
        }

        public override string ToString()
        {
            return $"{Title} ({Items.Count})";
        }
    }
}
