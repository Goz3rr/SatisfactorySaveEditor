using System;
using System.Windows;

using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveEditor.ViewModel.Struct;

using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;

using Vector = SatisfactorySaveParser.PropertyTypes.Structs.Vector;
using Vector3 = SatisfactorySaveParser.Structures.Vector3;

namespace SatisfactorySaveEditor.Cheats
{
    public class UndoDeleteEnemiesCheat : ICheat
    {
        public string Name => "Undo Delete enemies";

        private SaveObjectModel FindOrCreatePath(SaveObjectModel start, string[] path, int index = 0)
        {
            if (index == path.Length)
                return start;
            if (start.FindChild(path[index], false) == null)
                start.Items.Add(new SaveObjectModel(path[index]));
            return FindOrCreatePath(start.FindChild(path[index], false), path, index + 1);
        }

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave save)
        {
            var animalSpawners = rootItem.FindChild("BP_CreatureSpawner.BP_CreatureSpawner_C", false);
            if (animalSpawners == null)
            {
                MessageBox.Show("This save does not contain animals or it is corrupt.", "Cannot find animals", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            float offset = -50000;
            var hostPlayerModel = rootItem.FindChild("Char_Player.Char_Player_C", false);
            if (hostPlayerModel == null || hostPlayerModel.Items.Count < 1)
            {
                MessageBox.Show("This save does not contain a host player or it is corrupt.", "Cannot find host player", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            Vector3 playerPosition = ((SaveEntityModel)hostPlayerModel.Items[0]).Position;

            foreach (SaveObjectModel animalSpawner in animalSpawners.DescendantSelfViewModel)
            {
                var probablyEdited = ((SaveEntityModel)animalSpawner).Position.Z < -14200f;

                // Some crab hatchers are marked as CreatureSpawner instead of EnemySpawner and there is no other trace of the difference between enemy and friendly in the savefile
                //if (animalSpawner.Title.ToLower().Contains("enemy"))
                //{
                if (probablyEdited)
                    ((SaveEntityModel)animalSpawner).Position.Z -= offset; // Move the spawn under the map

                animalSpawner.FindField("mSpawnData", (ArrayPropertyViewModel arrayProperty) =>
                {
                    foreach (StructPropertyViewModel elem in arrayProperty.Elements)
                    {
                        if (probablyEdited)
                            ((Vector)((StructProperty)((DynamicStructDataViewModel)elem.StructData).Fields[0].Model).Data).Data.Z -= offset; // Move the spawn point under the map

                        // Set WasKilled to true so they don't respawn after deleting them
                        ((BoolPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[2]).Value = false;
                        // Set KilledOnDayNumber to a huge number (some far away animals respawn if the number is too small)
                        ((IntPropertyViewModel)((DynamicStructDataViewModel)elem.StructData).Fields[3]).Value = (int)0;
                    }
                });
            }

            return true;
        }
    }
}
