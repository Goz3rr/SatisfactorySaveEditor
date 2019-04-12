using System;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SatisfactorySaveEditor.Util;

namespace SatisfactorySaveEditor.ViewModel
{
    public class UpdateWindowViewModel : ViewModelBase
    {
        private readonly UpdateChecker.VersionInfo info;

        public RelayCommand<Window> OpenCommand { get; }
        public RelayCommand<Window> CloseCommand { get; }
        public RelayCommand<Window> DisableAutoCheckCommand { get; }

        public string Changelog => $"Satisfactory Save Editor {info.TagName}" + Environment.NewLine + info.Name + Environment.NewLine + Environment.NewLine + info.Changelog;

        public UpdateWindowViewModel(UpdateChecker.VersionInfo info)
        {
            this.info = info;

            OpenCommand = new RelayCommand<Window>(Open);
            CloseCommand = new RelayCommand<Window>(Close);
            DisableAutoCheckCommand = new RelayCommand<Window>(DisableAutoCheck);
        }

        private void Close(Window window)
        {
            window.Close();
        }

        private void DisableAutoCheck(Window window)
        {
            Properties.Settings.Default.AutoUpdate = false;
            MessageBox.Show("You have disabled automatic update checking. You can re-enable it in the preferences menu or manually check for updates in the 'Help' menu.", "Update", MessageBoxButton.OK);

            Properties.Settings.Default.Save();
            window.Close();
        }

        private void Open(Window window)
        {
            Process.Start(info.ReleaseUrl);
            window.Close();
        }
    }
}
