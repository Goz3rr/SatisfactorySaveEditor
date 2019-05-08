using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveEditor.ViewModel.Struct;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using System.Diagnostics;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class DeleteAnimalsCheat : ICheat
    {
        public string Name => "Delete Animals";

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
                animalSpawner.FindField("mSpawnData", (ArrayPropertyViewModel arrayProperty) =>
                {
                    foreach (StructPropertyViewModel elem in arrayProperty.Elements)
                        ((BoolPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[2]).Value = true;
                });

            // Actually delete the animals (RIP Lizzard doggos - is there any way to save them?)
            var animals = rootItem.FindChild("Creature", false);
            for (int i = animals.Items.Count - 1; i >= 0; i--)
                if (animals.Items[i].Title != "BP_CreatureSpawner.BP_CreatureSpawner_C")
                    animals.Items.Remove(animals.Items[i]);


            MessageBox.Show($"Deleted all animals. RIP Lizzard Doggos", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
