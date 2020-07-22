using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class Int64PropertyViewModel : SerializedPropertyViewModel
    {
        private readonly Int64Property model;

        private long value;

        public long Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Int64";

        public Int64PropertyViewModel(Int64Property intProperty) : base(intProperty)
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
