using System.Threading.Tasks;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.Cheats
{
    public class NoPowerCheat : ICheat
    {
        private readonly IDialogService dialogService;
        private readonly ISnackbarMessageQueue snackbar;
        public string Name => "No power";

        public NoPowerCheat()
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

            var numAdditionalSlots = gameState.FindOrCreateField<BoolPropertyViewModel>("mCheatNoPower");
            numAdditionalSlots.Value = !numAdditionalSlots.Value;
            snackbar.Enqueue($"Success! {(numAdditionalSlots.Value ? "Enabled" : "Disabled")} no power cheat", "Ok",
                () => { });
            return true;
        }
    }
}