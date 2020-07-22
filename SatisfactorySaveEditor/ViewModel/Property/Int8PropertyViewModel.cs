using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class Int8PropertyViewModel : SerializedPropertyViewModel
    {
        private readonly Int8Property model;

        private byte value;

        public byte Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Int8";

        public Int8PropertyViewModel(Int8Property intProperty) : base(intProperty)
        {
            model = intProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
