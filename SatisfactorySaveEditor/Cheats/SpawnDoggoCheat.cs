using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    class SpawnDoggoCheat : ICheat
    {
        public string Name => "Spawn Doggo at Host Player";

        private DeleteEnemiesCheat deleteEnemiesCheat; //uses the add doggo code from delete enemies to avoid duplicating code

        public SpawnDoggoCheat(DeleteEnemiesCheat deleter)
        {
            deleteEnemiesCheat = deleter;
        }
        
        public bool Apply(SaveObjectModel rootItem)
        {
            deleteEnemiesCheat.AddDoggo(rootItem);
            MessageBox.Show("Spawned 1 tamed Lizard Doggo at the host player.");
            return true;
        }
        
    }
}
