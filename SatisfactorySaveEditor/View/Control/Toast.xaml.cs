using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace SatisfactorySaveEditor.View.Control
{
    /// <summary>
    /// Interakční logika pro Toast.xaml
    /// </summary>
    public partial class Toast : UserControl
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(Icon), typeof(Toast), new PropertyMetadata(SystemIcons.Information));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(Toast), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Toast), new PropertyMetadata(string.Empty));

        public Icon Icon
        {
            get => (Icon) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Toast()
        {
            InitializeComponent();
        }
    }
}
