using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class MapPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly MapProperty model;

        public MapPropertyViewModel(MapProperty mapProperty) : base(mapProperty)
        {
            model = mapProperty;
        }

        public override void ApplyChanges()
        {
        }
    }
}
