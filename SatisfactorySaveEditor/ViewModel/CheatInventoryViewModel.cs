using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel
{
    class CheatInventoryViewModel : ViewModelBase
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }
        public int NumberChosen { get; set; } = int.MinValue; //default value in case the prompt box is x'd

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
            MessageBox.Show("Test " + NumberChosen);

            obj.Close();
        }

    }


}
