using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.Data;
using SatisfactorySaveParser.PropertyTypes;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using MaterialDesignThemes.Wpf;

namespace SatisfactorySaveEditor.Cheats
{
    public class ResearchUnlockCheat : ICheat
    {
        private readonly ISnackbarMessageQueue snackbar;
        private readonly IDialogService dialogService;
        public string Name => "Unlock all research";

        public ResearchUnlockCheat()
        {
            dialogService = ServiceLocator.Current.GetInstance<IDialogService>();
            snackbar = ServiceLocator.Current.GetInstance<ISnackbarMessageQueue>();
        }

        public async Task<bool> Apply(SaveObjectModel rootItem)
        {
            var tutorialManager = rootItem.FindChild("Persistent_Level:PersistentLevel.TutorialIntroManager", false);
            if (tutorialManager == null)
            {
                MissingTagMsg("TutorialIntroManager");
                return false;
            }

            var schematicManager = rootItem.FindChild("Persistent_Level:PersistentLevel.schematicManager", false);
            if (schematicManager == null)
            {
                MissingTagMsg("schematicManager");
                return false;
            }

            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                MissingTagMsg("GameState");
                return false;
            }

            var gamePhaseManager = rootItem.FindChild("Persistent_Level:PersistentLevel.GamePhaseManager", false);
            if (gamePhaseManager == null)
            {
                MissingTagMsg("GamePhaseManager");
                return false;
            }

            var tradingPostBuilt = tutorialManager.FindField<BoolPropertyViewModel>("mTradingPostBuilt");
            if (tradingPostBuilt == null)
            {
                await dialogService.ShowError("You should build a hub before attempting to unlock all research.",
                    "No hub found", "Ok",
                    () => { });
                return false;
            }

            tutorialManager.FindOrCreateField<EnumPropertyViewModel>("mPendingTutorial", f => f.Value = "EIntroTutorialSteps::ITS_DONE");
            tutorialManager.FindOrCreateField<IntPropertyViewModel>("mTradingPostLevel", f => f.Value = 5);
            CreateOrSetBoolField(tutorialManager, "mHasCompletedIntroTutorial", true);
            CreateOrSetBoolField(tutorialManager, "mHasCompletedIntroSequence", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep1Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep2Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep3Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep4Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep5Upgrade", true);


            // TODO: Set GameState->mScannableResources
            CreateOrSetBoolField(gameState, "mIsBuildingEfficiencyUnlocked", true);
            CreateOrSetBoolField(gameState, "mIsBuildingOverclockUnlocked", true);

            gamePhaseManager.FindOrCreateField<BytePropertyViewModel>("mGamePhase", f =>
            {
                f.Value = "EGP_EndGame";
                f.Type = "EGamePhase";
            });

            foreach (var field in schematicManager.Fields)
            {
                if (field.PropertyName == "mAvailableSchematics" || field.PropertyName == "mPurchasedSchematics")
                {
                    if (!(field is ArrayPropertyViewModel arrayField))
                    {
                        await dialogService.ShowError(
                            "Expected schematic data is of wrong type.\nThis means that the loaded save is probably corrupt. Aborting.",
                            "Wrong schematics type", "Ok", () => { });
                        return false;
                    }

                    foreach (var research in Research.GetResearches())
                    {
                        if (!arrayField.Elements.Cast<ObjectPropertyViewModel>().Any(e => e.Str2 == research.Path))
                        {
                            arrayField.Elements.Add(new ObjectPropertyViewModel(new ObjectProperty(null, "", research.Path)));
                        }
                    }
                }
            }

            snackbar.Enqueue("All research successfully unlocked.", "Ok", () => { });
            return true;
        }

        private async void MissingTagMsg(string tagName)
        {
            await dialogService.ShowError(
                $"This save does not contain a {tagName}.\nThis means that the loaded save is probably corrupt. Aborting.",
                $"Cannot find {tagName}", "Ok",
                () => { });
        }

        private BoolPropertyViewModel CreateOrSetBoolField(SaveObjectModel model, string fieldName, bool value)
        {
            return model.FindOrCreateField<BoolPropertyViewModel>(fieldName, f => f.Value = value);
        }
    }
}
