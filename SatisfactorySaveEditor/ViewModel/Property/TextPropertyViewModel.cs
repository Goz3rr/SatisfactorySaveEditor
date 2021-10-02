using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class TextPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly TextProperty model;

        private string value;

        public string Value
        {
            get => value;
            set { Set(() => Value, ref this.value, value); }
        }

        public override string ShortName => "Text";

        public TextPropertyViewModel(TextProperty textProperty) : base(textProperty)
        {
            model = textProperty;

            switch(textProperty.Text)
            {
                case BaseTextEntry baseText:
                    value = baseText.Value;
                    break;
                case NoneTextEntry baseText:
                    value = baseText.CultureInvariantString;
                    break;
            }
        }

        public override void ApplyChanges()
        {
            switch (model.Text)
            {
                case BaseTextEntry baseText:
                    baseText.Value = value;
                    break;
                case NoneTextEntry baseText:
                    baseText.CultureInvariantString = value;
                    break;
            }
        }
    }
}
