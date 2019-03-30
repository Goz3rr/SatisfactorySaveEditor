using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class TextPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly TextProperty model;

        private string value;
        private int unknown4;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public int Unknown4
        {
            get => unknown4;
            set { Set(() => Unknown4, ref this.unknown4, value); }
        }

        public TextPropertyViewModel(TextProperty textProperty) : base(textProperty)
        {
            model = textProperty;

            value = model.Value;
            unknown4 = model.Unknown4;
        }

        public override void ApplyChanges()
        {
            model.Value = value;
            model.Unknown4 = unknown4;
        }
    }
}
