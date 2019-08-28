using System;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.Util;

namespace SatisfactorySaveEditor.ViewModel
{
    public class UpdateWindowViewModel : ViewModelBase
    {
        private readonly UpdateChecker.VersionInfo info;
        private readonly ISnackbarMessageQueue snackbar;

        public RelayCommand OpenCommand => new RelayCommand(Open);
        public RelayCommand DisableAutoCheckCommand => new RelayCommand(DisableAutoCheck);

        public string Changelog => $"Satisfactory Save Editor {info.TagName}" + Environment.NewLine + info.Name + Environment.NewLine + Environment.NewLine + info.Changelog;

        public UpdateWindowViewModel(ISnackbarMessageQueue snackbar, UpdateChecker.VersionInfo info)
        {
            this.snackbar = snackbar;
            this.info = info;
        }

        private void DisableAutoCheck()
        {
            Properties.Settings.Default.AutoUpdate = false;
            snackbar.Enqueue("You have disabled automatic update checking. You can re-enable it in the preferences menu or manually check for updates in the 'Help' menu.", "Ok", () => { });

            Properties.Settings.Default.Save();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void Open()
        {
            Process.Start(info.ReleaseUrl);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}