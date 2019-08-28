using System.Threading.Tasks;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.Cheats
{
    public class NoCostCheat : ICheat
    {
        private readonly IDialogService dialogService;
        private readonly ISnackbarMessageQueue snackbar;
        public string Name => "No cost";

        public NoCostCheat()
        {
            dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            snackbar = ServiceLocator.Current.GetInstance<ISnackbarMessageQueue>();
        }

        public async Task<bool> Apply(SaveObjectModel rootItem)
        {
            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                await dialogService.ShowError("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.",
                    "Cannot find GameState", "Ok", () => { });
                return false;
            }

            var numAdditionalSlots = gameState.FindOrCreateField<BoolPropertyViewModel>("mCheatNoCost");
            numAdditionalSlots.Value = !numAdditionalSlots.Value;

            snackbar.Enqueue($"Success! {(numAdditionalSlots.Value ? "Enabled" : "Disabled")} no cost cheat", "Ok", () => { });
            return true;
        }
    }
}