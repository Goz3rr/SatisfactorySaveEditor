using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveEditor.ViewModel.Struct;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using System;
using System.Diagnostics;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class DeleteEnemiesCheat : ICheat
    {
        public string Name => "Delete enemies";

        public bool Apply(SaveObjectModel rootItem)
        {
            var animalSpawners = rootItem.FindChild("BP_CreatureSpawner.BP_CreatureSpawner_C", false);
            if (animalSpawners == null)
            {
                MessageBox.Show("This save does not contain animals or it is corrupt.", "Cannot find animals", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (SaveObjectModel animalSpawner in animalSpawners.DescendantSelfViewModel)
            {
                // Some crab hatchers are marked as CreatureSpawner instead of EnemySpawner and there is no other trace of the difference between enemy and friendly in the savefile
                //if (animalSpawner.Title.ToLower().Contains("enemy"))
                //{
                    animalSpawner.FindField("mSpawnData", (ArrayPropertyViewModel arrayProperty) =>
                    {
                        foreach (StructPropertyViewModel elem in arrayProperty.Elements)
                        {
                            // Set WasKilled to true so they don't respawn after deleting them
                            ((BoolPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[2]).Value = true;
                            // Set KilledOnDayNumber to a huge number (some crabs respawn if the number is too small)
                            ((IntPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[3]).Value = 1000000000;
                        }
                    });
                //}
            }

            // Delete the already spawned enemies
            var enemies = rootItem.FindChild("Creature", false).FindChild("Enemy", false);
            rootItem.Remove(enemies);


            MessageBox.Show($"Deleted all spawned enemies, and all unspawned creatures (of any kind).", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
