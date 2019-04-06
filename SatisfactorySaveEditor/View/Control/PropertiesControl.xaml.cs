using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SatisfactorySaveEditor.ViewModel.Property;

namespace SatisfactorySaveEditor.View.Control
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

        public ICommand RemovePropertyCommand
        {
            get => (ICommand)GetValue(RemovePropertyCommandProperty);
            set => SetValue(RemovePropertyCommandProperty, value);
        }

        public ObservableCollection<SerializedPropertyViewModel> Properties
        {
            get => (ObservableCollection<SerializedPropertyViewModel>)GetValue(PropertiesProperty);
            set => SetValue(PropertiesProperty, value);
        }

        public static readonly DependencyProperty AddPropertyCommandProperty = DependencyProperty.Register("AddPropertyCommand", typeof(ICommand), typeof(PropertiesControl));
        public static readonly DependencyProperty RemovePropertyCommandProperty = DependencyProperty.Register("RemovePropertyCommand", typeof(ICommand), typeof(PropertiesControl));
        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register("Properties", typeof(ObservableCollection<SerializedPropertyViewModel>), typeof(PropertiesControl));
    }
}
