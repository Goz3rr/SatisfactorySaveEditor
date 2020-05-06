using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;

namespace SatisfactorySaveEditor.ViewModel
{
    public class AboutWindowViewModel : ViewModelBase
    {
        public RelayCommand<string> OpenGitCommand { get; }

        public AboutWindowViewModel()
        {
            OpenGitCommand = new RelayCommand<string>(BrowserUtil.OpenBrowser);
        }
    }
}
