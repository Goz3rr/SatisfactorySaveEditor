using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class BytePropertyViewModel : SerializedPropertyViewModel
    {
        private readonly ByteProperty model;

        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public BytePropertyViewModel(ByteProperty byteProperty) : base(byteProperty)
        {
            model = byteProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
