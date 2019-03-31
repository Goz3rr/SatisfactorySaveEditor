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
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        // The new GUID makes sure we get a fresh instance every time, these windows don't have any persistent data
        public AddViewModel AddViewModel => ServiceLocator.Current.GetInstance<AddViewModel>(Guid.NewGuid().ToString());
        public CheatInventoryViewModel CheatInventoryViewModel => ServiceLocator.Current.GetInstance<CheatInventoryViewModel>(Guid.NewGuid().ToString());


        public static void Cleanup()
        {
        }
    }
}