using System.Reflection;
using System.Windows;

namespace SatisfactorySaveEditor.View
{
    /// <summary>
    /// Interakční logika pro AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent(); // Yeah, I COULD set up an entire viewmodel for a single string and a close button or I could do the sensible thing
            VersionText.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
