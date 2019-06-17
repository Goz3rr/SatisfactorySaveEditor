using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SatisfactorySaveEditor.ViewModel
{
    public class StringPromptViewModel : ViewModelBase
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        private string valueChosen;
        public string ValueChosen
        {
            get => valueChosen;
            set { Set(() => ValueChosen, ref valueChosen, value); }
        }

        private string oldValueDisplay;
        public string OldValueDisplay
        {
            get => oldValueDisplay;
            set { Set(() => OldValueDisplay, ref oldValueDisplay, value); }
        }

        
        private string windowTitle;
        private string promptMessage;
        private string promptInitialValue;

        

        public StringPromptViewModel(string windowTitle, string promptMessage, string promptInitialValue) 
            : this()
        {
            MessageBox.Show("Running 3 element constructor");
            this.windowTitle = windowTitle;
        }

        public StringPromptViewModel()
        {
            MessageBox.Show("Running regular element constructor");
            this.windowTitle = "String prompt";
            this.promptMessage = "Enter a string:";
            this.promptInitialValue = null;

            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);
        }

        private void Cancel(Window obj)
        {
            ValueChosen = null;
            obj.Close();
        }

        private void Ok(Window obj)
        {
            obj.Close();
        }
    }
}
