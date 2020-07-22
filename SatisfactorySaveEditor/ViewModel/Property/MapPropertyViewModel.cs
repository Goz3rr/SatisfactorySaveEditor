using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class MapPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly MapProperty model;

        public override string ShortName => "Map";

        public MapPropertyViewModel(MapProperty mapProperty) : base(mapProperty)
        {
            model = mapProperty;
        }

        public override void ApplyChanges()
        {
        }
    }
}
