using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class DoublePropertyViewModel : SerializedPropertyViewModel
    {
        private readonly DoubleProperty model;

        private double value;

        public double Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Double";

        public DoublePropertyViewModel(DoubleProperty doubleProperty) : base(doubleProperty)
        {
            model = doubleProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
