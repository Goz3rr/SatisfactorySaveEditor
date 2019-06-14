using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace SourceCodeMessage.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<Window> OpenReleasesCommand { get; }
        public RelayCommand<Window> CloseCommand { get; }

        public MainViewModel()
        {
            OpenReleasesCommand = new RelayCommand<Window>(OpenReleases);
            CloseCommand = new RelayCommand<Window>(Close);
        }

        private void OpenReleases(Window obj)
        {
            System.Diagnostics.Process.Start("https://github.com/Goz3rr/SatisfactorySaveEditor/releases");
            Application.Current.Shutdown();
        }

        private void Close(Window obj)
        {
            Application.Current.Shutdown();
        }
    }
}