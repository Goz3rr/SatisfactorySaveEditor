using GalaSoft.MvvmLight;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public abstract class SerializedPropertyViewModel : ViewModelBase
    {
        public readonly SerializedProperty Model;

        public string PropertyName => Model.PropertyName;

        /// <summary>
        /// Gets or sets the index of this property in an array
        /// Leave null for properties outside arrays
        /// </summary>
        public string Index { get; set; }

        protected SerializedPropertyViewModel(SerializedProperty serializedProperty)
        {
            Model = serializedProperty;
        }

        public abstract void ApplyChanges();
    }
}
