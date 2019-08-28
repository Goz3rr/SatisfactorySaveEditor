using System;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.ViewModel
{
    public class DialogService : IDialogService
    {
        private const string DialogIdentifier = "RootDialog";
        public async Task ShowError(string message, string title, string buttonText, Action afterHideCallback)
        {
            CheckDialogOpen();
            await DialogHost.Show(new ConfirmViewModel()
            {
                Title = title,
                Message = message,
                CancelText = buttonText,
                IconKind = "WarningBox",
                ShowConfirm = false
            }, DialogIdentifier);
        }

        public async Task ShowError(Exception error, string title, string buttonText, Action afterHideCallback)
        {
            CheckDialogOpen();
            await DialogHost.Show(new ConfirmViewModel()
            {
                Title = title,
                Message = error.ToString(),
                CancelText = buttonText,
                IconKind = "WarningBox",
                ShowConfirm = false
            }, DialogIdentifier);
        }

        public async Task ShowMessage(string message, string title)
        {
            CheckDialogOpen();
            await DialogHost.Show(new ConfirmViewModel()
            {
                Title = title,
                Message = message,
                CancelText = "Ok",
                IconKind = "About",
                ShowConfirm = false
            }, DialogIdentifier);
        }

        public async Task ShowMessage(string message, string title, string buttonText, Action afterHideCallback)
        {
            CheckDialogOpen();
            await DialogHost.Show(new ConfirmViewModel()
            {
                Title = title,
                Message = message,
                CancelText = buttonText,
                IconKind = "About",
                ShowConfirm = false
            }, DialogIdentifier);
        }

        public async Task<bool> ShowMessage(string message, string title, string buttonConfirmText, string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            CheckDialogOpen();
            var dialogResult = await DialogHost.Show(new ConfirmViewModel()
            {
                Title = title,
                Message = message,
                ConfirmText = buttonConfirmText,
                CancelText = buttonCancelText,
                IconKind = "QuestionMarkBox",
                ShowConfirm = true
            }, DialogIdentifier);
            return (bool)dialogResult;
        }

        public async Task<object> ShowDialog<T>(object obj, DialogClosingEventHandler closingEventHandler)
        {
            CheckDialogOpen();
            return await DialogHost.Show((T)obj, DialogIdentifier, closingEventHandler);
        }

        public async Task<object> ShowDialog<T>(object obj)
        {
            CheckDialogOpen();
            return await DialogHost.Show((T)obj, DialogIdentifier);
        }

        public async Task ShowMessageBox(string message, string title)
        {
            await ShowMessage(message, title);
        }

        private void CheckDialogOpen()
        {
            var mvm = ServiceLocator.Current.GetInstance<MainViewModel>();
            if (mvm.DialogOpen) mvm.DialogOpen = false;
        }
    }
}