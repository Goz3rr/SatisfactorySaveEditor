using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SatisfactorySaveEditor.ViewModel
{
    public class UnlockResearchWindowViewModel : ViewModelBase
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        public RelayCommand<IList> AddOneCommand { get; }
        public RelayCommand<IList> RemoveOneCommand { get; }
        public RelayCommand AddAllCommand { get; }
        public RelayCommand AddAllAlternativesCommand { get; }
        public RelayCommand RemoveAllCommand { get; }

        private ObservableCollection<string> available;
        public ObservableCollection<string> Available
        {
            get => available;
            set { Set(() => Available, ref available, value); }
        }

        private ObservableCollection<string> unlocked;
        public ObservableCollection<string> Unlocked
        {
            get => unlocked;
            set { Set(() => Unlocked, ref unlocked, value); }
        }

        public UnlockResearchWindowViewModel()
        {
            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);

            AddOneCommand = new RelayCommand<IList>(AddOne, list => list?.Count > 0);
            RemoveOneCommand = new RelayCommand<IList>(RemoveOne, list => list?.Count > 0);
            AddAllCommand = new RelayCommand(AddAll, () => Available?.Count() > 0);
            AddAllAlternativesCommand = new RelayCommand(AddAllAlternatives, () => Available?.Any(x => x.Contains(@"/Alternate/")) ?? false);
            RemoveAllCommand = new RelayCommand(RemoveAll, () => Unlocked?.Count() > 0);
        }

        private void Cancel(Window window)
        {
            window.DialogResult = false;
            window.Close();
        }

        private void Ok(Window window)
        {
            window.DialogResult = true;
            window.Close();
        }

        private void AddOne(IList items)
        {
            if (items == null)
                return;

            foreach (var item in items.Cast<string>().Reverse().ToList())
            {
                Available.Remove(item);
                Unlocked.Insert(0, item);
            }
        }

        private void RemoveOne(IList items)
        {
            if (items == null)
                return;

            foreach (var item in items.Cast<string>().Reverse().ToList())
            {
                Unlocked.Remove(item);
                Available.Insert(0, item);
            }
        }

        private void AddAll()
        {
            foreach (var item in Available.Reverse().ToList())
            {
                Unlocked.Insert(0, item);
                Available.Remove(item);
            }
        }

        private void AddAllAlternatives()
        {
            foreach (var item in Available.Where(x => x.Contains(@"/Alternate/")).Reverse().ToList())
            {
                Unlocked.Insert(0, item);
                Available.Remove(item);
            }
        }

        private void RemoveAll()
        {
            foreach (var item in Unlocked.Reverse().ToList())
            {
                Available.Insert(0, item);
                Unlocked.Remove(item);
            }
        }
    }
}
