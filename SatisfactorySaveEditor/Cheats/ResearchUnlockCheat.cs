using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.Data;
using SatisfactorySaveParser.PropertyTypes;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class ResearchUnlockCheat : ICheat
    {
        public string Name => "Unlock all research";

        public bool Apply(SaveObjectModel rootItem)
        {
            var tutorialManager = rootItem.FindChild("Persistent_Level:PersistentLevel.TutorialIntroManager", false);
            if (tutorialManager == null)
            {
                MessageBox.Show("This save does not contain a TutorialIntroManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find TutorialIntroManager", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var schematicManager = rootItem.FindChild("Persistent_Level:PersistentLevel.schematicManager", false);
            if (schematicManager == null)
            {
                MessageBox.Show("This save does not contain a schematicManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find schematicManager", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_0", false);
            if (gameState == null)
            {
                MessageBox.Show("This save does not contain a GameState.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GameState", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var gamePhaseManager = rootItem.FindChild("Persistent_Level:PersistentLevel.GamePhaseManager", false);
            if (gamePhaseManager == null)
            {
                MessageBox.Show("This save does not contain a GamePhaseManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find GamePhaseManager", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var tradingPostBuilt = tutorialManager.FindField<BoolPropertyViewModel>("mTradingPostBuilt");
            if (tradingPostBuilt == null)
            {
                MessageBox.Show("You should build a hub before attempting to unlock all research", "No hub found", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            tutorialManager.FindOrCreateField<EnumPropertyViewModel>("mPendingTutorial", f => f.Value = "EIntroTutorialSteps::ITS_DONE");
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mHasCompletedIntroTutorial", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mHasCompletedIntroSequence", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mDidStep1Upgrade", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mDidStep2Upgrade", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mDidStep3Upgrade", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mDidStep4Upgrade", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mDidStep5Upgrade", f => f.Value = true);
            tutorialManager.FindOrCreateField<IntPropertyViewModel>("mTradingPostLevel", f => f.Value = 5);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mHasCompletedIntroTutorial", f => f.Value = true);
            tutorialManager.FindOrCreateField<BoolPropertyViewModel>("mHasCompletedIntroTutorial", f => f.Value = true);

            // TODO: Set GameState->mScannableResources
            gameState.FindOrCreateField<BoolPropertyViewModel>("mIsBuildingEfficiencyUnlocked", f => f.Value = true);
            gameState.FindOrCreateField<BoolPropertyViewModel>("mIsBuildingOverclockUnlocked", f => f.Value = true);

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
                        MessageBox.Show("Expected schematic data is of wrong type.\nThis means that the loaded save is probably corrupt. Aborting.", "Wrong schematics type", MessageBoxButton.OK, MessageBoxImage.Error);
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

            MessageBox.Show("All research successfully unlocked.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
