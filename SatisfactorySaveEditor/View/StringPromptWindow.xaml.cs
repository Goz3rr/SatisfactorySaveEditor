using System.Windows;

namespace SatisfactorySaveEditor.View
{
    /// <summary>
    /// Interaction logic for StringPromptWindow.xaml
    /// </summary>
    public partial class StringPromptWindow : Window
    {
        public StringPromptWindow()
        {
            InitializeComponent();
            StringBox.Focus();
        }
    }
}
