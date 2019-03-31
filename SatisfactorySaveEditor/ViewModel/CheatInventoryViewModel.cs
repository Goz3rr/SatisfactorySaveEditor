using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SatisfactorySaveEditor.ViewModel
{
    public class CheatInventoryViewModel : ViewModelBase
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        private int numberChosen;
        public int NumberChosen
        {
            get => numberChosen;
            set { Set(() => NumberChosen, ref numberChosen, value); }
        }

        private int oldSlotsDisplay;
        public int OldSlotsDisplay
        {
            get => oldSlotsDisplay;
            set { Set(() => OldSlotsDisplay, ref oldSlotsDisplay, value); }
        }

        public CheatInventoryViewModel()
        {
            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);
        }

        private void Cancel(Window obj)
        {
            NumberChosen = int.MinValue;
            obj.Close();
        }

        private void Ok(Window obj)
        {
            obj.Close();
        }
    }
}
