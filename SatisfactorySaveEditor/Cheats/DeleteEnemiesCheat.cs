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

            // Set WasKilled to true so they don't respawn after deleting them
            foreach (SaveObjectModel animalSpawner in animalSpawners.DescendantSelfViewModel)
                if (animalSpawner.Title.ToLower().Contains("enemy"))
                {
                    animalSpawner.FindField("mSpawnData", (ArrayPropertyViewModel arrayProperty) =>
                    {
                        foreach (StructPropertyViewModel elem in arrayProperty.Elements)
                            ((BoolPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[2]).Value = true;
                    });
                }

            // Actually delete the animals
            var enemies = rootItem.FindChild("Creature", false).FindChild("Enemy", false);
            rootItem.Remove(enemies);


            MessageBox.Show($"Deleted all enemies.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
