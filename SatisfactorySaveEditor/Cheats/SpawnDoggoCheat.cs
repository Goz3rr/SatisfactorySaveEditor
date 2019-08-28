using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;
using SatisfactorySaveEditor.View.Control;
using SatisfactorySaveEditor.View.Dialogs;

namespace SatisfactorySaveEditor.Cheats
{
    class SpawnDoggoCheat : ICheat
    {
        private readonly DialogService dialogService;
        private readonly ISnackbarMessageQueue snackbar;
        public string Name => "Spawn Doggos...";

        private DeleteEnemiesCheat deleteEnemiesCheat; //uses the add doggo code from delete enemies to avoid duplicating code

        public SpawnDoggoCheat(DeleteEnemiesCheat deleter)
        {
            deleteEnemiesCheat = deleter;
            dialogService = (DialogService)ServiceLocator.Current.GetInstance<IDialogService>();
            snackbar = ServiceLocator.Current.GetInstance<ISnackbarMessageQueue>();
        }

        public async Task<bool> Apply(SaveObjectModel rootItem)
        {
            var dialog = new StringPromptDialog();
            var cvm = (StringPromptViewModel) dialog.DataContext;
            cvm.Title = "Enter doggo count";
            cvm.PromptMessage = "Count (integer):";
            cvm.ValueChosen = "1";
            cvm.OldValueMessage = "";
            if (!(await dialogService.ShowDialog<StringPromptDialog>(dialog) is string valueChosen)) return false;
            try
            {
                var doggoCount = int.Parse(valueChosen);

                if (doggoCount > 0)
                {
                    int counter;
                    bool pastSuccess = true; //don't keep running the loop if one run fails
                    for (counter = 0; counter < doggoCount && pastSuccess; counter++)
                    {
                        pastSuccess = await deleteEnemiesCheat.AddDoggo(rootItem);
                    }

                    if (pastSuccess)
                    {
                        snackbar.Enqueue($"Spawned {counter} doggo(s) at the host player", "Ok", () => { });
                        return true;
                    }

                    //failed to spawn some doggos for some reason
                    return false;
                }

                await dialogService.ShowError($"You can't spawn {doggoCount} doggos.", "Error!", "Ok", () => { });
                return false;
            }
            catch (Exception)
            {
                await dialogService.ShowError($"Could not parse: {valueChosen}", "Error!", "Ok", () => { });
                return false;
            }
        }

    }
}
