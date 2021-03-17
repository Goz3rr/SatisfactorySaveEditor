using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace SatisfactorySaveEditor.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<AddViewModel>();
            SimpleIoc.Default.Register<CheatInventoryViewModel>();
            SimpleIoc.Default.Register<StringPromptViewModel>();
            SimpleIoc.Default.Register<PreferencesWindowViewModel>();
            SimpleIoc.Default.Register<FillViewModel>();
            SimpleIoc.Default.Register<UnlockResearchWindowViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        // The new GUID makes sure we get a fresh instance every time, these windows don't have any persistent data
        public AddViewModel AddViewModel => ServiceLocator.Current.GetInstance<AddViewModel>(Guid.NewGuid().ToString());
        public CheatInventoryViewModel CheatInventoryViewModel => ServiceLocator.Current.GetInstance<CheatInventoryViewModel>(Guid.NewGuid().ToString());
        public StringPromptViewModel StringPromptViewModel => ServiceLocator.Current.GetInstance<StringPromptViewModel>(Guid.NewGuid().ToString());
        public PreferencesWindowViewModel PreferencesWindowViewModel => ServiceLocator.Current.GetInstance<PreferencesWindowViewModel>(Guid.NewGuid().ToString());
        public FillViewModel FillViewModel => ServiceLocator.Current.GetInstance<FillViewModel>(Guid.NewGuid().ToString());
        public UnlockResearchWindowViewModel UnlockResearchWindowViewModel => ServiceLocator.Current.GetInstance<UnlockResearchWindowViewModel>(Guid.NewGuid().ToString());

        public static void Cleanup()
        {
        }
    }
}