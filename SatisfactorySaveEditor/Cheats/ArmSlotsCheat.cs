using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class ArmSlotsCheat : ICheat
    {
        public string Name => "Set arm slot count...";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.UnlockSubsystem", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a UnlockSubsystem.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find UnlockSubsystem", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var numAdditionalArmSlots = gameState.FindOrCreateField<IntPropertyViewModel>("mNumTotalArmEquipmentSlots");

            var dialog = new CheatInventoryWindow //reusing CheatInventory since it's the same kind of prompt
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (CheatInventoryViewModel)dialog.DataContext;
            cvm.NumberChosen = numAdditionalArmSlots.Value;
            cvm.OldSlotsDisplay = numAdditionalArmSlots.Value;
            dialog.ShowDialog();

            if (cvm.NumberChosen < 0 || cvm.NumberChosen == numAdditionalArmSlots.Value)
            {
                MessageBox.Show("Arm slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            numAdditionalArmSlots.Value = cvm.NumberChosen;
            string message = $"Arm slot count set to {cvm.NumberChosen} slots.\n\nPlease note that you may need to complete at least one milestone that unlocks arm slots for your changes to take effect.";
            if (numAdditionalArmSlots.Value > 6)
                message += "\n\nArm slot counts greater than 6 cause visual problems in the inventory screen.";
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
