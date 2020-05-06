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
            SimpleIoc.Default.Register<AboutWindowViewModel>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public PreferencesWindowViewModel PreferencesWindowViewModel => ServiceLocator.Current.GetInstance<PreferencesWindowViewModel>();
        public AboutWindowViewModel AboutWindowViewModel => ServiceLocator.Current.GetInstance<AboutWindowViewModel>();

        public static void Cleanup()
        {
        }
    }
}