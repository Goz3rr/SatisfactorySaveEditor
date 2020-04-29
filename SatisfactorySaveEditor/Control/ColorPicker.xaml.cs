using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveEditor.Control
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

        public FColor Color
        {
            get => (FColor)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(FColor), typeof(ColorPicker));

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PreviewRectangle.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }
    }
}
