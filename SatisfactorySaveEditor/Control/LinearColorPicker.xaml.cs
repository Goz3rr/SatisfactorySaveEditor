using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using SatisfactorySaveParser.Game.Structs.Native;

namespace SatisfactorySaveEditor.Control
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

        public FLinearColor LinearColor
        {
            get => (FLinearColor)GetValue(LinearColorProperty);
            set => SetValue(LinearColorProperty, value);
        }

        public static readonly DependencyProperty LinearColorProperty = DependencyProperty.Register("LinearColor", typeof(FLinearColor), typeof(LinearColorPicker));

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            PreviewRectangle.GetBindingExpression(Shape.FillProperty)?.UpdateTarget();
        }
    }
}
