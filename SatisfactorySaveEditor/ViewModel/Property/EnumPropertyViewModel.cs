using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class EnumPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly EnumProperty model;

        private int value;

        public int Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public EnumPropertyViewModel(EnumProperty enumProperty) : base(enumProperty)
        {
            model = enumProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
