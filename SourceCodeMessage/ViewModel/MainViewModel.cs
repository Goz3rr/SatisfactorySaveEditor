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
            //Robb, Goz3rr, and virusek20 have the login info for this bit.ly account if needed
            System.Diagnostics.Process.Start("http://bit.ly/SSE_Wrong_Download");
            Application.Current.Shutdown();
        }

        private void Close(Window obj)
        {
            Application.Current.Shutdown();
        }
    }
}