using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class UnlockMapCheat : ICheat
    {
        public string Name => "Unlock map";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.UnlockSubsystem", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain an UnlockSubsystem.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find UnlockSubsystem", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var isMapUnlocked = gameState.FindOrCreateField<BoolPropertyViewModel>("mIsMapUnlocked");
            isMapUnlocked.Value = true;

            MessageBox.Show("Map unlocked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}