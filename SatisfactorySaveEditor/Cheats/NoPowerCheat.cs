using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class NoPowerCheat : ICheat
    {
        public string Name => "Toggle no power cost";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_*", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var numAdditionalSlots = gameState.FindOrCreateField<BoolPropertyViewModel>("mCheatNoPower");
            numAdditionalSlots.Value = !numAdditionalSlots.Value;
            MessageBox.Show((numAdditionalSlots.Value ? "Enabled no power cheat.\nNote that buildings still need to be connected to an electrical network to function" : "Disabled no power cheat"), "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            return true;
        }
    }
}
