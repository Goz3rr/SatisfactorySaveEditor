using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class BytePropertyViewModel : SerializedPropertyViewModel
    {
        private readonly ByteProperty model;

        private string type;
        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public string Type
        {
            get => type;
            set { Set(() => Type, ref type, value); }
        }

        public override string ShortName => "Byte";

        public BytePropertyViewModel(ByteProperty byteProperty) : base(byteProperty)
        {
            model = byteProperty;

            value = model.Value;
            type = model.Type;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
            model.Type = type;
        }
    }
}
