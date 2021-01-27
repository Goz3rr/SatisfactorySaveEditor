using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace SatisfactorySaveEditor.ViewModel
{
    public class CheatInventoryViewModel : ObservableObject
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        private int numberChosen;
        public int NumberChosen
        {
            get => numberChosen;
            set { SetProperty(ref numberChosen, value); }
        }

        private int oldSlotsDisplay;
        public int OldSlotsDisplay
        {
            get => oldSlotsDisplay;
            set { SetProperty(ref oldSlotsDisplay, value); }
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
