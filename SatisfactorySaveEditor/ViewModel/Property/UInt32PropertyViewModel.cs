using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class UInt32PropertyViewModel : SerializedPropertyViewModel
    {
        private readonly UInt32Property model;

        private uint value;

        public uint Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "UInt32";

        public UInt32PropertyViewModel(UInt32Property uintProperty) : base(uintProperty)
        {
            model = uintProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
