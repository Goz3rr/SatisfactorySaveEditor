using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using SatisfactorySaveEditor.Model;

namespace SatisfactorySaveEditor.ViewModel
{
    public class FillViewModel : ViewModelBase
    {
        private ResourceType selectedItem;
        public ResourceType SelectedItem
        {
            get => selectedItem;
            set
            {
                Set(() => SelectedItem, ref selectedItem, value);
                RaisePropertyChanged(nameof(CanConfirm));
            }
        }

        public bool IsConfirmed { get; set; }

        public List<ResourceType> ItemTypes => ResourceTypes.RESOURCES;

        public RelayCommand<Window> OkCommand => new RelayCommand<Window>(Confirmed);

        public RelayCommand<Window> CancelCommand => new RelayCommand<Window>(Cancelled);

        private void Confirmed(Window window)
        {
            IsConfirmed = true;
            window?.Close();
        }

        private void Cancelled(Window window)
        {
            window?.Close();
        }
        public bool CanConfirm
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedItem.ItemPath)) return false;
                return SelectedItem.Quantity > 0;
            }
        }
    }
}