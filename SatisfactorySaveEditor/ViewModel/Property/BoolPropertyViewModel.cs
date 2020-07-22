using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class BoolPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly BoolProperty model;

        private bool value;

        public bool Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Boolean";

        public BoolPropertyViewModel(BoolProperty boolProperty) : base(boolProperty)
        {
            model = boolProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
