using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser.PropertyTypes;

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
            set
            {
                Set(() => OldSlotsDisplay, ref oldSlotsDisplay, value);
            }
        }
        public bool CanConfirm
        {
            get
            {
                return NumberChosen >= 0;
            }
        }

        private int oldSlotsDisplay = 4242;
        public int OldSlotsDisplay
        {
            get
            {
                return oldSlotsDisplay;
            }
            set
            {
                Set(() => OldSlotsDisplay, ref oldSlotsDisplay, value);
            }
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
