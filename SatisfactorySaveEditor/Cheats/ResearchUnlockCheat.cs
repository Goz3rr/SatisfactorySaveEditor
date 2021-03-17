using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;
using SatisfactorySaveParser.Data;
using SatisfactorySaveParser.PropertyTypes;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class ResearchUnlockCheat : ICheat
    {
        public string Name => "Unlock research";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
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

            var gameState = rootItem.FindChild("Persistent_Level:PersistentLevel.BP_GameState_C_*", false);
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

            var unlockSystem = rootItem.FindChild("Persistent_Level:PersistentLevel.UnlockSubsystem", false);
            if (unlockSystem == null)
            {
                MissingTagMsg("UnlockSubsystem");
                return false;
            }

            var tradingPostBuilt = tutorialManager.FindField<BoolPropertyViewModel>("mTradingPostBuilt");
            if (tradingPostBuilt == null)
            {
                MessageBox.Show("You should build a hub before attempting to unlock all research.", "No hub found", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var availableSchematics = schematicManager.FindField<ArrayPropertyViewModel>("mAvailableSchematics");
            var purchasedSchematics = schematicManager.FindField<ArrayPropertyViewModel>("mPurchasedSchematics");

            var researches = Research.GetResearches().Select(r => r.Path);

            var purchasedList = purchasedSchematics.Elements.Cast<ObjectPropertyViewModel>().Select(o => o.Str2);
            var availableList = researches.Union(availableSchematics.Elements.Cast<ObjectPropertyViewModel>().Select(o => o.Str2)).Except(purchasedList);

            var window = new UnlockResearchWindow()
            {
                Owner = Application.Current.MainWindow
            };
            var vm = (UnlockResearchWindowViewModel)window.DataContext;
            vm.Available = new ObservableCollection<string>(availableList);
            vm.Unlocked = new ObservableCollection<string>(purchasedList);

            if (!window.ShowDialog().Value)
                return false;

            purchasedSchematics.Elements.Clear();
            foreach (var item in vm.Unlocked)
            {
                purchasedSchematics.Elements.Add(new ObjectPropertyViewModel(new ObjectProperty(null, "", item)));

                //if (!availableList.Any(x => x == item))
                //{
                //    availableSchematics.Elements.Add(new ObjectPropertyViewModel(new ObjectProperty(null, "", item)));
                //}
            }

            //clear PaidOffSchematic and ActiveSchematic fields so the progress indicator in the top right doesn't linger after unlocking
            var paidOffSchematicField = schematicManager.FindField<ArrayPropertyViewModel>("mPaidOffSchematic");
            var activeSchematicField = schematicManager.FindField<ObjectPropertyViewModel>("mActiveSchematic");
            schematicManager.RemovePropertyCommand.Execute(paidOffSchematicField);
            schematicManager.RemovePropertyCommand.Execute(activeSchematicField);

            tutorialManager.FindOrCreateField<EnumPropertyViewModel>("mPendingTutorial", f => f.Value = "EIntroTutorialSteps::ITS_DONE");
            tutorialManager.FindOrCreateField<IntPropertyViewModel>("mTradingPostLevel", f => f.Value = 6);
            CreateOrSetBoolField(tutorialManager, "mHasCompletedIntroTutorial", true);
            CreateOrSetBoolField(tutorialManager, "mHasCompletedIntroSequence", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep1Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep1_5Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep2Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep3Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep4Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidStep5Upgrade", true);
            CreateOrSetBoolField(tutorialManager, "mDidPickUpIronOre", true);
            CreateOrSetBoolField(tutorialManager, "mDidDismantleDropPod", true);
            CreateOrSetBoolField(tutorialManager, "mDidEquipStunSpear", true);
            CreateOrSetBoolField(tutorialManager, "mDidOpenCodex", true);

            // TODO: Set GameState->mScannableResources
            CreateOrSetBoolField(unlockSystem, "mIsMapUnlocked", true);
            CreateOrSetBoolField(unlockSystem, "mIsBuildingEfficiencyUnlocked", true);
            CreateOrSetBoolField(unlockSystem, "mIsBuildingOverclockUnlocked", true);

            gamePhaseManager.FindOrCreateField<BytePropertyViewModel>("mGamePhase", f =>
            {
                f.Value = "EGP_EndGame";
                f.Type = "EGamePhase";
            });

            MessageBox.Show("Research successfully unlocked.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void MissingTagMsg(string tagName)
        {
            MessageBox.Show($"This save does not contain a {tagName}.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find " + tagName, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private BoolPropertyViewModel CreateOrSetBoolField(SaveObjectModel model, string fieldName, bool value)
        {
            return model.FindOrCreateField<BoolPropertyViewModel>(fieldName, f => f.Value = value);
        }
    }
}
