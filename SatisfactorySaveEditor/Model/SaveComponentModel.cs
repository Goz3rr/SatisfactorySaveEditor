using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.View.Control;
using SatisfactorySaveEditor.View.Dialogs;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveEditor.ViewModel.Struct;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveEditor.Model
{
    public class SaveComponentModel : SaveObjectModel
    {
        private string parentEntityName;
        private readonly DialogService dialogService;
        private readonly ISnackbarMessageQueue snackbar;

        public string ParentEntityName
        {
            get => parentEntityName;
            set { Set(() => ParentEntityName, ref parentEntityName, value); }
        }

        public RelayCommand FillInventoryCommand => new RelayCommand(FillInventory);

        public RelayCommand EmptyInventoryCommand => new RelayCommand(EmptyInventory);

        public SaveComponentModel(SaveComponent sc) : base(sc)
        {
            ParentEntityName = sc.ParentEntityName;
            dialogService = (DialogService) ServiceLocator.Current.GetInstance<IDialogService>();
            snackbar = ServiceLocator.Current.GetInstance<ISnackbarMessageQueue>();
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            var model = (SaveComponent) Model;

            model.ParentEntityName = ParentEntityName;
        }

        private async void FillInventory()
        {
            var result = await dialogService.ShowDialog<FillDialog>(new FillDialog());
            if (!(result is FillViewModel fvm)) return;

            var inv = FindField<ArrayPropertyViewModel>("mInventoryStacks");
            foreach (StructPropertyViewModel element in inv.Elements)
            {
                DynamicStructDataViewModel structData = (DynamicStructDataViewModel)element.StructData;
                InventoryItem item = (InventoryItem)((StructPropertyViewModel)structData.Fields[0]).StructData;
                IntPropertyViewModel numItems = (IntPropertyViewModel)structData.Fields[1];
                item.ItemType = fvm.SelectedItem.ItemPath;
                numItems.Value = fvm.SelectedItem.Quantity;
            }
            ApplyChanges();
        }

        private void EmptyInventory()
        {
            var inv = FindField<ArrayPropertyViewModel>("mInventoryStacks");
            foreach (StructPropertyViewModel element in inv.Elements)
            {
                DynamicStructDataViewModel structData = (DynamicStructDataViewModel)element.StructData;
                InventoryItem item = (InventoryItem)((StructPropertyViewModel)structData.Fields[0]).StructData;
                IntPropertyViewModel numItems = (IntPropertyViewModel)structData.Fields[1];
                item.ItemType = string.Empty;
                numItems.Value = 0;
            }
            ApplyChanges();
            snackbar.Enqueue($"Inventory for container {Title} emptied", "Ok", () => { });
        }
    }
}
