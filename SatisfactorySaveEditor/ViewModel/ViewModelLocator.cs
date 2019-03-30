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
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public AddViewModel AddViewModel => ServiceLocator.Current.GetInstance<AddViewModel>(Guid.NewGuid().ToString());
        public CheatInventoryViewModel CheatInventoryViewModel => ServiceLocator.Current.GetInstance<CheatInventoryViewModel>();


        public static void Cleanup()
        {
        }
    }
}