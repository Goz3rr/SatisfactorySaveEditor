using SatisfactorySaveEditor.ViewModel.Struct;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class StructPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly StructProperty model;

        private object structData; // TODO: Rest of the owl, implement view models for structs

        public string Type => model.Type;

        public object StructData
        {
            get => structData;
            set { Set(() => StructData, ref structData, value); }
        }

        public override string ShortName => $"Struct ({Type})";

        public StructPropertyViewModel(StructProperty structProperty) : base(structProperty)
        {
            model = structProperty;

            if (model.Data is DynamicStructData dsd) structData = new DynamicStructDataViewModel(dsd);
            else structData = model.Data;
        }

        public override void ApplyChanges()
        {
            if (structData is DynamicStructDataViewModel dsdvm) dsdvm.ApplyChanges();
            else model.Data = (IStructData) structData;
        }
    }
}
