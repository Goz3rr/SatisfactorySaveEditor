using GalaSoft.MvvmLight;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public abstract class SerializedPropertyViewModel : ViewModelBase
    {
        public readonly SerializedProperty Model;

        public string PropertyName => Model.PropertyName;

        protected SerializedPropertyViewModel(SerializedProperty serializedProperty)
        {
            Model = serializedProperty;
        }

        public abstract void ApplyChanges();
    }
}
