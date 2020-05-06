using SatisfactorySaveEditor.Service.Toast;
using SatisfactorySaveParser.Game.Blueprint;
using SatisfactorySaveParser.Save;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SatisfactorySaveEditor.Cheat
{
    [CheatInfo("Toggle no build cost", "Removes build cost of all buildings")]
    public class NoCostCheat : ICheat
    {
        private readonly ToastService _toastService;

        public NoCostCheat(ToastService toastService)
        {
            _toastService = toastService;
        }

        public bool CanExecute(FGSaveSession session, SaveObject rightPanelSelected, List<SaveObject> multiSelected)
        {
            var gameState = session.Objects.OfType<FGGameState>().FirstOrDefault();
            if (gameState == null)
            {
                _toastService.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", SystemIcons.Error);
                return false;
            }

            return true;
        }

        public void Execute(FGSaveSession session, SaveObject rightPanelSelected, List<SaveObject> multiSelected)
        {
            FGGameState gameState = session.Objects.OfType<FGGameState>().First();
            gameState.CheatNoCost = !gameState.CheatNoCost;

            _toastService.Show($"{(gameState.CheatNoCost ? "Enabled" : "Disabled")} no building cost cheat", "Success", SystemIcons.Information);
        }
    }
}
