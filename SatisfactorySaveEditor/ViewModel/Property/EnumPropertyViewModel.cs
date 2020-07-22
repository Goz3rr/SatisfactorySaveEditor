using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class EnumPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly EnumProperty model;

        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Enum";

        public EnumPropertyViewModel(EnumProperty enumProperty) : base(enumProperty)
        {
            model = enumProperty;

            value = model.Name;
        }

        public override void ApplyChanges()
        {
            model.Name = value;
        }
    }
}
