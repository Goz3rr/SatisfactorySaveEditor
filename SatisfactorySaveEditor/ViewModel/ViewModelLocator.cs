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
            SimpleIoc.Default.Register<PreferencesWindowViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public PreferencesWindowViewModel PreferencesWindowViewModel => ServiceLocator.Current.GetInstance<PreferencesWindowViewModel>();

        public static void Cleanup()
        {
        }
    }
}