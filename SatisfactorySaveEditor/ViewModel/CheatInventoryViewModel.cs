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
                Set(() => NumberChosen, ref numberChosen, value);
                RaisePropertyChanged(() => CanConfirm);
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
                oldSlotsDisplay = value;
                RaisePropertyChanged("OldSlotsDisplay");
            }
        }

        //public int NumberChosen { get; set; }// = int.MinValue; //default value in case the prompt box is x'd
        //public string OldSlotsDisplay = "(Currently X slots.)"; //for display on the window

        public CheatInventoryViewModel()
        {
            //NumberChosen = oldCount;
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
            //MessageBox.Show("Test " + NumberChosen);
            obj.Close();
        }

    }


}
