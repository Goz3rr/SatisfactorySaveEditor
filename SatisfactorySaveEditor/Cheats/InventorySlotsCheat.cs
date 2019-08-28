using System.Threading.Tasks;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.Cheats
{
    public class InventorySlotsCheat : ICheat
    {
        private readonly ISnackbarMessageQueue snackbar;
        public string Name => "Set bonus inventory slots...";

        public InventorySlotsCheat()
        {
            snackbar = ServiceLocator.Current.GetInstance<ISnackbarMessageQueue>();
        }

        public async Task<bool> Apply(SaveObjectModel rootItem)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                snackbar.Enqueue("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Ok", () => { });
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
                snackbar.Enqueue("Bonus inventory slot count unchanged", "Ok", () => { });
                return false;
            }

            numAdditionalSlots.Value = cvm.NumberChosen;
            snackbar.Enqueue($"Bonus inventory set to {cvm.NumberChosen} slots.", "Ok", () => { });
            return true;
        }
    }
}