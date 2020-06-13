using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class InventorySlotsCheat : ICheat
    {
        public string Name => "Set inventory slot count...";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.UnlockSubsystem", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a UnlockSubsystem.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find UnlockSubsystem", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var numAdditionalSlots = gameState.FindOrCreateField<IntPropertyViewModel>("mNumTotalInventorySlots");

            var dialog = new CheatInventoryWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (CheatInventoryViewModel)dialog.DataContext;
            cvm.NumberChosen = numAdditionalSlots.Value;
            cvm.OldSlotsDisplay = numAdditionalSlots.Value;
            dialog.ShowDialog();

            if (cvm.NumberChosen < 0 || cvm.NumberChosen == numAdditionalSlots.Value)
            {
                MessageBox.Show("Inventory slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            numAdditionalSlots.Value = cvm.NumberChosen;
            MessageBox.Show($"Inventory slot count set to {cvm.NumberChosen} slots.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
