using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.Message;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.ViewModel
{
    public class PreferencesViewModel : ViewModelBase
    {
        private bool canApply;
        private bool autoUpdate;
        private bool autoBackup;
        private bool drawerEnabled;

        private bool darkModeEnabled;

        public bool AutoUpdate
        {
            get => autoUpdate;
            set
            {
                Set(() => AutoUpdate, ref autoUpdate, value);
                canApply = true;
            }
        }

        public bool AutoBackup
        {
            get => autoBackup;
            set
            {
                Set(() => AutoBackup, ref autoBackup, value);
                canApply = true;
            }
        }

        public bool DrawerEnabled
        {
            get => drawerEnabled;
            set
            {
                Set(() => DrawerEnabled, ref drawerEnabled, value);
                canApply = true;
            }
        }

        public bool DarkModeEnabled
        {
            get => darkModeEnabled;
            set
            {
                Set(() => DarkModeEnabled, ref darkModeEnabled, value);
                canApply = true;
            }
        }

        public RelayCommand AcceptCommand => new RelayCommand(Accept);
        public RelayCommand ApplyCommand => new RelayCommand(Apply, CanApply);
        public RelayCommand CancelCommand => new RelayCommand(Cancel);
        public RelayCommand<bool> ToggleDarkModeCommand => new RelayCommand<bool>(ToggleDarkMode);

        public PreferencesViewModel()
        {
            autoUpdate = Properties.Settings.Default.AutoUpdate;
            autoBackup = Properties.Settings.Default.AutoBackup;
            drawerEnabled = Properties.Settings.Default.DrawerEnabled;
            darkModeEnabled = Properties.Settings.Default.DarkModeEnabled;
        }

        private void Accept()
        {
            Apply();
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private void Apply()
        {
            Properties.Settings.Default.AutoUpdate = autoUpdate;
            Properties.Settings.Default.AutoBackup = autoBackup;
            Properties.Settings.Default.DrawerEnabled = drawerEnabled;
            Properties.Settings.Default.DarkModeEnabled = darkModeEnabled;
            Messenger.Default.Send(new DrawerEnabledMessage(drawerEnabled));

            Properties.Settings.Default.Save();
            canApply = false;
        }

        private void Cancel()
        {
            ToggleDarkMode(Properties.Settings.Default.DarkModeEnabled);
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        private bool CanApply()
        {
            return canApply;
        }

        private void ToggleDarkMode(bool darkMode)
        {
            Messenger.Default.Send(new DarkModeEnabledMessage(darkMode));
        }
    }
}