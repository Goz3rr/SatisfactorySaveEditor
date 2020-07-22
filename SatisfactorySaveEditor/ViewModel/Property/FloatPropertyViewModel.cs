using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class FloatPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly FloatProperty model;

        private float value;

        public float Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Float";

        public FloatPropertyViewModel(FloatProperty floatProperty) : base(floatProperty)
        {
            model = floatProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
