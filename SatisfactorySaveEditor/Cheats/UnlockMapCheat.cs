using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.PropertyTypes;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class UnlockMapCheat : ICheat
    {
        public string Name => "Unlock map";

        public bool Apply(SaveObjectModel rootItem)
        {
            var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (cheatObject == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mIsMapUnlocked") is BoolPropertyViewModel mapUnlocked)
            {
                mapUnlocked.Value = true;
            }
            else
            {
                cheatObject.Fields.Add(new BoolPropertyViewModel(new BoolProperty("mIsMapUnlocked")
                {
                    Value = true
                }));
            }

            MessageBox.Show("Map unlocked", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}