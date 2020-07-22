using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel.Property
{
    public class SetPropertyViewModel : SerializedPropertyViewModel
    {
        private readonly SetProperty model;

        private bool isExpanded;

        public override string ShortName => "Set";

        public RelayCommand AddElementCommand { get; }
        public RelayCommand<SerializedPropertyViewModel> RemoveElementCommand { get; }

        public ObservableCollection<SerializedPropertyViewModel> Elements { get; }

        public string Type => model.Type;

        public bool IsExpanded
        {
            get => isExpanded;
            set { Set(() => IsExpanded, ref isExpanded, value); }
        }

        public SetPropertyViewModel(SetProperty setProperty) : base(setProperty)
        {
            model = setProperty;

            Elements = new ObservableCollection<SerializedPropertyViewModel>(setProperty.Elements.Select(PropertyViewModelMapper.Convert));
            for (var i = 0; i < Elements.Count; i++) Elements[i].Index = i.ToString();

            AddElementCommand = new RelayCommand(AddElement);
            RemoveElementCommand = new RelayCommand<SerializedPropertyViewModel>(RemoveElement);

            IsExpanded = Elements.Count <= 3;
        }

        private void AddElement()
        {
            // TODO: Is copying the last PropertyName ok?
            var property = AddViewModel.CreateProperty(AddViewModel.FromStringType(Type), Elements.Last().PropertyName);
            var viewModel = PropertyViewModelMapper.Convert(property);
            viewModel.Index = Elements.Count.ToString();

            Elements.Add(viewModel);
        }

        private void RemoveElement(SerializedPropertyViewModel property)
        {
            Elements.Remove(property);
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
