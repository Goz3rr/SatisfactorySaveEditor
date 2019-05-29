using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.View.Control
{
    /// <summary>
    /// Interakční logika pro Vector3Control.xaml
    /// </summary>
    public partial class Vector3Control : UserControl
    {
        public Vector3Control()
        {
            InitializeComponent();
        }

        public Vector3 Vector
        {
            get => (Vector3)GetValue(VectorProperty);
            set => SetValue(VectorProperty, value);
        }

        public static readonly DependencyProperty VectorProperty = DependencyProperty.Register("Vector", typeof(Vector3), typeof(Vector3Control));
    }
}
