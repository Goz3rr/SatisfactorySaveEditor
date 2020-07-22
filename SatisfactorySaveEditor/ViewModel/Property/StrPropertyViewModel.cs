using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class StrPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly StrProperty model;

        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "String";

        public StrPropertyViewModel(StrProperty strProperty) : base(strProperty)
        {
            model = strProperty;

            value = model.Value;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
        }
    }
}
