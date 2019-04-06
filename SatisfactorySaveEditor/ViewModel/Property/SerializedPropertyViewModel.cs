using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SatisfactorySaveParser.PropertyTypes;
using System.Windows;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public abstract class SerializedPropertyViewModel : ViewModelBase
    {
        public readonly SerializedProperty Model;

        public string PropertyName => Model.PropertyName;

        public RelayCommand CopyPropertyNameCommand { get; }

        protected SerializedPropertyViewModel(SerializedProperty serializedProperty)
        {
            Model = serializedProperty;
            CopyPropertyNameCommand = new RelayCommand(CopyPropertyName);
        }

        public abstract void ApplyChanges();

        private void CopyPropertyName()
        {
            Clipboard.SetText(PropertyName);
        }
    }
}
