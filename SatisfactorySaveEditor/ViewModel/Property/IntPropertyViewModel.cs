using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class IntPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly IntProperty model;

        private int value;

        public int Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Int";

        public IntPropertyViewModel(IntProperty intProperty) : base(intProperty)
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
