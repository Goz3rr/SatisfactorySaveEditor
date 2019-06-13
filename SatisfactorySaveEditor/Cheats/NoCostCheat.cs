using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class NoCostCheat : ICheat
    {
        public string Name => "No cost";

        public bool Apply(SaveObjectModel rootItem)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var numAdditionalSlots = gameState.FindOrCreateField<BoolPropertyViewModel>("mCheatNoCost");
            numAdditionalSlots.Value = !numAdditionalSlots.Value;
            MessageBox.Show($"{(numAdditionalSlots.Value ? "Enabled" : "Disabled")} no cost cheat", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }
    }
}
