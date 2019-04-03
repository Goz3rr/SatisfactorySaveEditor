using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.View.Control
{
    /// <summary>
    /// Interakční logika pro ColorPicker.xaml
    /// </summary>
    public partial class LinearColorPicker : UserControl
    {
        public LinearColorPicker()
        {
            InitializeComponent();
        }

        public LinearColor LinearColor
        {
            get => (LinearColor)GetValue(LinearColorProperty);
            set => SetValue(LinearColorProperty, value);
        }

        public static readonly DependencyProperty LinearColorProperty = DependencyProperty.Register("LinearColor", typeof(LinearColor), typeof(LinearColorPicker));

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PreviewRectangle.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }
    }
}
