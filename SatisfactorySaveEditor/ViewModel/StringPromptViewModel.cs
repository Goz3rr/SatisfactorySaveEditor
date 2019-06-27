using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

//TODO: Make pressing enter in the text box trigger Ok()
namespace SatisfactorySaveEditor.ViewModel
{
    public class StringPromptViewModel : ViewModelBase
    {
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        private string windowTitle = "String Prompt";
        public string WindowTitle
        {
            get => windowTitle;
            set { Set(() => WindowTitle, ref windowTitle, value); }
        }

        private string promptMessage = "Prompt String:";
        public string PromptMessage
        {
            get => promptMessage;
            set { Set(() => PromptMessage, ref promptMessage, value); }
        }

        private string valueChosen;
        public string ValueChosen
        {
            get => valueChosen;
            set { Set(() => ValueChosen, ref valueChosen, value); }
        }

        private string oldValueMessage = "(1)Old Value Message\n(2)\n(3)\n(4)\n(5)";
        public string OldValueMessage
        {
            get => oldValueMessage;
            set { Set(() => OldValueMessage, ref oldValueMessage, value); }
        }

        public StringPromptViewModel()
        {
            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);
        }

        private void Cancel(Window obj)
        {
            ValueChosen = "cancel";
            obj.Close();
        }

        private void Ok(Window obj)
        {
            obj.Close();
        }
    }
}
