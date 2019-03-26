using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.View
{
    /// <summary>
    /// Interakční logika pro PropertiesControl.xaml
    /// </summary>
    public partial class PropertiesControl : UserControl
    {
        public PropertiesControl()
        {
            InitializeComponent();
        }

        public ICommand AddPropertyCommand
        {
            get => (ICommand)GetValue(AddPropertyCommandProperty);
            set => SetValue(AddPropertyCommandProperty, value);
        }

        public ObservableCollection<SerializedProperty> Properties
        {
            get => (ObservableCollection<SerializedProperty>)GetValue(PropertiesProperty);
            set => SetValue(PropertiesProperty, value);
        }

        public static readonly DependencyProperty AddPropertyCommandProperty = DependencyProperty.Register("AddPropertyCommand", typeof(ICommand), typeof(PropertiesControl));
        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(ObservableCollection<SerializedProperty>), typeof(PropertiesControl));
    }
}
