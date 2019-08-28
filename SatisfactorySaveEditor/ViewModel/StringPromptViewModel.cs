using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

//TODO: Make pressing enter in the text box trigger Ok()
namespace SatisfactorySaveEditor.ViewModel
{
    public class StringPromptViewModel : ViewModelBase
    {
        private string title = "String Prompt";
        public string Title
        {
            get => title;
            set { Set(() => Title, ref title, value); }
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
    }
}
