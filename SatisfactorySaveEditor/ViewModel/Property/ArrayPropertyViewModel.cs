using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class ArrayPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly ArrayProperty model;

        public RelayCommand AddElementCommand { get; }

        public ObservableCollection<SerializedPropertyViewModel> Elements { get; }

        public string Type => model.Type;

        public ArrayPropertyViewModel(ArrayProperty arrayProperty) : base(arrayProperty)
        {
            model = arrayProperty;

            Elements = new ObservableCollection<SerializedPropertyViewModel>(arrayProperty.Elements.Select(PropertyViewModelMapper.Convert));

            AddElementCommand = new RelayCommand(AddElement);
        }

        private void AddElement()
        {
            var property = AddViewModel.CreateProperty(AddViewModel.FromStringType(Type), $"Element {Elements.Count}");
            Elements.Add(PropertyViewModelMapper.Convert(property));
        }

        public override void ApplyChanges()
        {
            model.Elements.Clear();
            foreach (var element in Elements)
            {
                element.ApplyChanges();
                model.Elements.Add(element.Model);
            }
        }
    }
}
