using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class InterfacePropertyViewModel : SerializedPropertyViewModel
    {
        private readonly InterfaceProperty model;

        private string str1;
        private string str2;

        public string Str1
        {
            get => str1;
            set { Set(() => Str1, ref str1, value); }
        }

        public string Str2
        {
            get => str2;
            set { Set(() => Str2, ref str2, value); }
        }

        public override string ShortName => "Interface";

        public InterfacePropertyViewModel(InterfaceProperty interfaceProperty) : base(interfaceProperty)
        {
            model = interfaceProperty;

            str1 = model.LevelName;
            str2 = model.PathName;
        }

        public override void ApplyChanges()
        {
            model.LevelName = str1;
            model.PathName = str2;
        }
    }
}
