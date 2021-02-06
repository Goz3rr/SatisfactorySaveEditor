using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
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
        public string Name => "Spawn doggos...";

        private DeleteEnemiesCheat deleteEnemiesCheat; //uses the add doggo code from delete enemies to avoid duplicating code

        public SpawnDoggoCheat(DeleteEnemiesCheat deleter)
        {
            deleteEnemiesCheat = deleter;
        }
        
        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            int doggocount = 1;

            var dialog = new StringPromptWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (StringPromptViewModel)dialog.DataContext;
            cvm.WindowTitle = "Enter doggo count";
            cvm.PromptMessage = "Count (integer):";
            cvm.ValueChosen = "1";
            cvm.OldValueMessage = "";
            dialog.ShowDialog();

            try
            {
                doggocount = int.Parse(cvm.ValueChosen);
                //MessageBox.Show("" + doggocount);

                if (doggocount > 0)
                {
                    int counter;
                    bool pastSuccess = true; //don't keep running the loop if one run fails
                    for (counter = 0; counter < doggocount && pastSuccess; counter++)
                    {
                        pastSuccess = deleteEnemiesCheat.AddDoggo(rootItem, saveGame);
                    }

                    if (pastSuccess)
                    {
                        MessageBox.Show("Spawned " + counter + " doggo(s) at the host player.");
                        return true;
                    }
                    else
                    {
                        //failed to spawn some doggos for some reason
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("You can't spawn " + doggocount + " doggos.");
                    return false;
                }
            }
            catch (Exception)
            {
                if (!(cvm.ValueChosen == "cancel"))
                {
                    MessageBox.Show("Could not parse: " + cvm.ValueChosen);
                }
                return false;
            }
        }
        
    }
}
