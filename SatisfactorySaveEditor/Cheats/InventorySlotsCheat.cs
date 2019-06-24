using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class InventorySlotsCheat : ICheat
    {
        public string Name => "Set bonus inventory slots...";

        public bool Apply(SaveObjectModel rootItem)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var numAdditionalSlots = gameState.FindOrCreateField<IntPropertyViewModel>("mNumAdditionalInventorySlots");

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
                MessageBox.Show("Bonus inventory slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            numAdditionalSlots.Value = cvm.NumberChosen;
            MessageBox.Show($"Bonus inventory set to {cvm.NumberChosen} slots.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
