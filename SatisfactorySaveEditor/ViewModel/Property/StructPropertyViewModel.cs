using System.Collections.ObjectModel;
using System.Linq;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class StructPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly StructProperty model;

        public string Type => model.Type;

        public ObservableCollection<SerializedPropertyViewModel> Fields { get; }

        public StructPropertyViewModel(StructProperty structProperty) : base(structProperty)
        {
            model = structProperty;

            if (model.Data is DynamicStructData dsd)
            {
                Fields = new ObservableCollection<SerializedPropertyViewModel>(dsd.Fields.Select(PropertyViewModelMapper.Convert));
            }
            else Fields = new ObservableCollection<SerializedPropertyViewModel>();
        }

        public override void ApplyChanges()
        {
            foreach (var field in Fields) field.ApplyChanges();
        }
    }
}
