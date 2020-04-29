using NLog;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SatisfactorySaveEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs ev)
        {
            base.OnStartup(ev);

            AppDomain.CurrentDomain.UnhandledException += (s, e) => log.Error(e.ExceptionObject);
            DispatcherUnhandledException += (s, e) => log.Error(e.Exception);
            TaskScheduler.UnobservedTaskException += (s, e) => log.Error(e.Exception);
        }
    }
}
