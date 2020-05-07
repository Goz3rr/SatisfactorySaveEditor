using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using SatisfactorySaveEditor.Service.Cheat;
using SatisfactorySaveEditor.Service.Toast;
using SatisfactorySaveEditor.Service.Save;

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

            SimpleIoc.Default.Register<ToastService>();
            SimpleIoc.Default.Register<CheatService>();
            SimpleIoc.Default.Register<SaveIOService>();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public PreferencesWindowViewModel PreferencesWindowViewModel => ServiceLocator.Current.GetInstance<PreferencesWindowViewModel>();
        public AboutWindowViewModel AboutWindowViewModel => ServiceLocator.Current.GetInstance<AboutWindowViewModel>();

        public static void Cleanup()
        {
        }
    }
}