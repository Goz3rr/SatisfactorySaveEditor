using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;

using SatisfactorySaveParser;

using System;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class RevealMapCheat : ICheat
    {
        public string Name => "Uncover entire map";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var mapManager = rootItem.FindChild("Persistent_Level:PersistentLevel.MapManager", false);
            if (mapManager == null)
            {
                MessageBox.Show("This save does not contain a MapManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find MapManager", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            //map data is an Array(Byte) containing 1048576 elements ranging from 0 to 255, where 255 is fully revealed.
            var fogOfWarRawData = mapManager.FindOrCreateField<ArrayPropertyViewModel>("mFogOfWarRawData");

            if (!(fogOfWarRawData is ArrayPropertyViewModel))
            {
                MessageBox.Show("FogOfWarRawData is of wrong type.\nThis means that the loaded save is probably corrupt. Aborting.", "Wrong property type", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (fogOfWarRawData.Elements.Count != 1048576)
                MessageBox.Show($"Expected 1048576 fog of war elements to be present, but there were actually {fogOfWarRawData.Elements.Count}.\nThis does not necessarily mean that the process will fail. Please report this error on the Github Issues page regardless.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            int mapRevealThreshold = 0;

            var dialog = new StringPromptWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (StringPromptViewModel)dialog.DataContext;
            cvm.WindowTitle = "Enter map reveal threshold";
            cvm.PromptMessage = "Enter a number between 0 and 255 (integer):";
            cvm.ValueChosen = "255";
            cvm.OldValueMessage = "Sets the reveal state of every map region to the value you choose.\n255 is fully explored\n0 is entirely unexplored";
            dialog.ShowDialog();

            try
            {
                mapRevealThreshold = int.Parse(cvm.ValueChosen);
                if (mapRevealThreshold >= 0 && mapRevealThreshold <= 255)
                {
                    for (int i = 0; i < fogOfWarRawData.Elements.Count; i++)
                    {
                        ((BytePropertyViewModel)fogOfWarRawData.Elements[i]).Value = $"{mapRevealThreshold}";
                    }
                }
                else
                {
                    MessageBox.Show("You must enter a number between 0 and 255.");
                    return false;
                }
            }
            catch (Exception)
            {
                if (!(cvm.ValueChosen == "cancel"))
                {
                    MessageBox.Show($"Could not parse: {cvm.ValueChosen}");
                }
                return false;
            }

            MessageBox.Show("Map data uncovered. Remember to enable the map as well if desired.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}