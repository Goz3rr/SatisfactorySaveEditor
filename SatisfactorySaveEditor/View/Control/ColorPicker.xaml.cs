using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Color = SatisfactorySaveParser.PropertyTypes.Structs.Color;

namespace SatisfactorySaveEditor.View.Control
{
    /// <summary>
    /// Interakční logika pro ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker));

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PreviewRectangle.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }
    }
}
