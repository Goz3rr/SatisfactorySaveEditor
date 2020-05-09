using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;

namespace SatisfactorySaveEditor.Service.Toast
{
    public class ToastService
    {
        public ObservableCollection<ToastViewModel> ActiveToasts { get; } = new ObservableCollection<ToastViewModel>();

        public void Show(string text, string title, Icon icon, TimeSpan? lifetime = null)
        {
            var toast = new ToastViewModel
            {
                Title = title,
                Text = text,
                Icon = icon
            };

            if (lifetime.HasValue) toast.Lifespan = lifetime.Value;

            Task.Run(async () =>
            {
                await Task.Delay(toast.Lifespan);
                Application.Current.Dispatcher.Invoke(() => ActiveToasts.Remove(toast));
            });

            Application.Current.Dispatcher.Invoke(() => ActiveToasts.Add(toast));
        }

        public void Clear()
        {
            Application.Current.Dispatcher.Invoke(() => ActiveToasts.Clear());
        }
    }
}
