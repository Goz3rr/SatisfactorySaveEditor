using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.PropertyTypes;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class InventorySlotsCheat : ICheat
    {
        public string Name => "Set bonus inventory slots";

        public bool Apply(SaveObjectModel rootItem)
        {
            var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (cheatObject == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            int oldSlots = 0;
            int requestedSlots = 0;
            if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntPropertyViewModel inventorySize)
            {
                oldSlots = inventorySize.Value;
            }

            CheatInventoryWindow window = new CheatInventoryWindow(oldSlots)
            {
                Owner = Application.Current.MainWindow
            };
            CheatInventoryViewModel cvm = (CheatInventoryViewModel)window.DataContext;
            cvm.NumberChosen = oldSlots;
            cvm.OldSlotsDisplay = oldSlots;
            window.ShowDialog();
            requestedSlots = cvm.NumberChosen;


            if (requestedSlots < 0 || requestedSlots == oldSlots) //TryParse didn't find a number, or cancel was clicked on the inputbox
            {
                MessageBox.Show("Bonus inventory slot count unchanged", "Unchanged", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else //TryParse found a number to use
            {
                if (cheatObject.Fields.FirstOrDefault(f => f.PropertyName == "mNumAdditionalInventorySlots") is IntPropertyViewModel inventorySize2)
                {
                    inventorySize2.Value = requestedSlots;
                }
                else
                {
                    cheatObject.Fields.Add(new IntPropertyViewModel(new IntProperty("mNumAdditionalInventorySlots")
                    {
                        Value = requestedSlots
                    }));
                }

                MessageBox.Show("Bonus inventory set to " + requestedSlots + " slots.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
        }
    }
}
