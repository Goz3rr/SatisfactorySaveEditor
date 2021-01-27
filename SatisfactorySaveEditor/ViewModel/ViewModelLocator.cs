using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace SatisfactorySaveEditor.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<MainViewModel>();

            serviceCollection.AddTransient<AddViewModel>();
            serviceCollection.AddTransient<CheatInventoryViewModel>();
            serviceCollection.AddTransient<StringPromptViewModel>();
            serviceCollection.AddTransient<PreferencesWindowViewModel>();
            serviceCollection.AddTransient<FillViewModel>();

            Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());
        }

        public MainViewModel MainViewModel => Ioc.Default.GetRequiredService<MainViewModel>();

        // These return a fresh instance every time since they were configured with AddTransient, these windows don't have any persistent data
        public AddViewModel AddViewModel => Ioc.Default.GetRequiredService<AddViewModel>();
        public CheatInventoryViewModel CheatInventoryViewModel => Ioc.Default.GetRequiredService<CheatInventoryViewModel>();
        public StringPromptViewModel StringPromptViewModel => Ioc.Default.GetRequiredService<StringPromptViewModel>();
        public PreferencesWindowViewModel PreferencesWindowViewModel => Ioc.Default.GetRequiredService<PreferencesWindowViewModel>();
        public FillViewModel FillViewModel => Ioc.Default.GetRequiredService<FillViewModel>();

        public static void Cleanup()
        {
        }
    }
}