using System.Windows;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace SatisfactorySaveEditor.ViewModel
{
    public class PreferencesWindowViewModel : ObservableObject
    {
        private bool canApply;
        private bool autoUpdate;
        private bool autoBackup;
        //private int backupCount; //Future


        public bool AutoUpdate
        {
            get => autoUpdate;
            set
            {
                SetProperty(ref autoUpdate, value);
                CanApply = true;
            }
        }

        public bool AutoBackup
        {
            get => autoBackup;
            set
            {
                SetProperty(ref autoBackup, value);
                CanApply = true;
            }
        }

        //Future
        /*public int BackupCount
        {
            get => backupCount;
            set
            {
                Set(() => BackupCount, ref backupCount, value);
                Set(() => CanApply, ref canApply, true);
            }
        }*/

        public bool CanApply
        {
            get => canApply;
            private set => SetProperty(ref canApply, value);
        }

        public RelayCommand<Window> AcceptCommand { get; }
        public RelayCommand ApplyCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        public PreferencesWindowViewModel()
        {
            AcceptCommand = new RelayCommand<Window>(Accept);
            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand<Window>(Cancel);

            autoUpdate = Properties.Settings.Default.AutoUpdate;
            autoBackup = Properties.Settings.Default.AutoBackup;
        }

        private void Accept(Window window)
        {
            Apply();
            window.Close();
        }

        private void Apply()
        {
            Properties.Settings.Default.AutoUpdate = autoUpdate;
            Properties.Settings.Default.AutoBackup = autoBackup;

            Properties.Settings.Default.Save();
            CanApply = false;
        }

        private void Cancel(Window window)
        {
            window.Close();
        }
    }
}
