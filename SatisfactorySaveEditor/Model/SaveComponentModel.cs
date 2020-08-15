using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using SatisfactorySaveEditor.View;
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
        }

        public override void ApplyChanges()
        {
            base.ApplyChanges();

            var model = (SaveComponent)Model;

            model.ParentEntityName = ParentEntityName;
        }

        public ArrayPropertyViewModel Inventory
        {
           get => FindField<ArrayPropertyViewModel>("mInventoryStacks");
        }

        public override bool MatchesFilter(string filter)
        {
            return base.MatchesFilter(filter) || MatchesFilterInventory(filter);
        }

        private bool MatchesFilterInventory(string filter)
        {

            return Inventory?.Elements.Cast<StructPropertyViewModel>().Any(element =>
            {
                DynamicStructDataViewModel structData = (DynamicStructDataViewModel)element.StructData;
                InventoryItem item = (InventoryItem)((StructPropertyViewModel) structData.Fields[0]).StructData;

                return item.ItemType.ToLower(CultureInfo.InvariantCulture).Contains(filter);
            }) ?? false;
        }

        private void FillInventory()
        {
            FillWindow dialog = new FillWindow();
            FillViewModel fvm = (FillViewModel) dialog.DataContext;
            dialog.ShowDialog();

            if(!fvm.IsConfirmed) return;
            ArrayPropertyViewModel inv = this.Inventory;
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
            var inv = this.Inventory;
            foreach (StructPropertyViewModel element in inv.Elements)
            {
                DynamicStructDataViewModel structData = (DynamicStructDataViewModel)element.StructData;
                InventoryItem item = (InventoryItem)((StructPropertyViewModel)structData.Fields[0]).StructData;
                IntPropertyViewModel numItems = (IntPropertyViewModel)structData.Fields[1];
                item.ItemType = string.Empty;
                numItems.Value = 0;
            }
            ApplyChanges();
            MessageBox.Show($"Inventory for storage {Title} emptied", "Inventory Emptied");
        }
    }
}
