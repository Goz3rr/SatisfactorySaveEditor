using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<ISnackbarMessageQueue>(() => new SnackbarMessageQueue());

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<AddViewModel>();
            SimpleIoc.Default.Register<CheatInventoryViewModel>();
            SimpleIoc.Default.Register<StringPromptViewModel>();
            SimpleIoc.Default.Register<PreferencesViewModel>();
            SimpleIoc.Default.Register<FillViewModel>();
            SimpleIoc.Default.Register<MassDismantleViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        // The new GUID makes sure we get a fresh instance every time, these windows don't have any persistent data
        public AddViewModel AddViewModel => ServiceLocator.Current.GetInstance<AddViewModel>(Guid.NewGuid().ToString());
        public CheatInventoryViewModel CheatInventoryViewModel => ServiceLocator.Current.GetInstance<CheatInventoryViewModel>(Guid.NewGuid().ToString());
        public StringPromptViewModel StringPromptViewModel => ServiceLocator.Current.GetInstance<StringPromptViewModel>(Guid.NewGuid().ToString());
        public PreferencesViewModel PreferencesViewModel => ServiceLocator.Current.GetInstance<PreferencesViewModel>(Guid.NewGuid().ToString());
        public FillViewModel FillViewModel => ServiceLocator.Current.GetInstance<FillViewModel>(Guid.NewGuid().ToString());
        public MassDismantleViewModel MassDismantleViewModel => ServiceLocator.Current.GetInstance<MassDismantleViewModel>(Guid.NewGuid().ToString());

        public static void Cleanup()
        {
        }
    }
}