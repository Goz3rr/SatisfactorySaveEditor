using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace SatisfactorySaveEditor.Model
{
    public class SaveNodeItem : ObservableObject
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
