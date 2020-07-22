using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class NamePropertyViewModel : SerializedPropertyViewModel
    {
        private readonly NameProperty model;

        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Name";

        public NamePropertyViewModel(NameProperty nameProperty) : base(nameProperty)
        {
            model = nameProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
