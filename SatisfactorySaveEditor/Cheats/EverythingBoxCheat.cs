using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.View;
using SatisfactorySaveEditor.ViewModel;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using SatisfactorySaveParser.Data;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    class EverythingBoxCheat : ICheat
    {
        public string Name => "Create crate with all items...";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {

            var hostPlayerModel = rootItem.FindChild("Char_Player.Char_Player_C", false);
            if (hostPlayerModel == null || hostPlayerModel.Items.Count < 1)
            {
                MessageBox.Show("This save does not contain a host player or it is corrupt.\nTry loading and re-saving the save from within the game.", "Cannot find host player", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            var playerEntityModel = (SaveEntityModel)hostPlayerModel.Items[0];

            int itemStackQuantity = 500;

            var dialog = new StringPromptWindow
            {
                Owner = Application.Current.MainWindow
            };
            var cvm = (StringPromptViewModel)dialog.DataContext;
            cvm.WindowTitle = "Enter quantity of each item to spawn";
            cvm.PromptMessage = "Count (integer):";
            cvm.ValueChosen = "500";
            cvm.OldValueMessage = "";
            dialog.ShowDialog();

            try
            {
                itemStackQuantity = int.Parse(cvm.ValueChosen);

                if (itemStackQuantity <= 0)
                {
                    MessageBox.Show("The quantity you entered is invalid.");
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

            ArrayProperty inventory = new ArrayProperty("mInventoryStacks")
            {
                Type = "StructProperty"
            };

            HashSet<string> resourceStrings = new HashSet<string>();
            HashSet<string> radioactiveStrings = new HashSet<string>();

            //dump all resources in a hashset first to remove duplicates TODO consider making a hashmap to numbers so they maintain order
            foreach (var resource in Resource.GetResources())
            {
                if (resource.IsRadioactive)
                    radioactiveStrings.Add(resource.Path);
                else
                    resourceStrings.Add(resource.Path);
            }

            //Populate the inventory with every item in ResourcesUnfiltered.xml
            foreach (var resource in resourceStrings)
            {
                if (!radioactiveStrings.Contains(resource))//because duplicates can be in the xml file in order, still need to check every string here for radioactivity
                {
                    //MessageBox.Show($"Processing resource {resource}");
                    byte[] bytes = MassDismantleCheat.PrepareForParse(resource, itemStackQuantity); //reuse mass dismantle cheat's parsing method
                    using (MemoryStream ms = new MemoryStream(bytes))
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        SerializedProperty prop = SerializedProperty.Parse(reader, saveGame.Header.BuildVersion);
                        inventory.Elements.Add(prop);
                    }
                }
            }

            string skipped = "";
            foreach (string radioactiveResource in radioactiveStrings)
            {
                skipped += $"{radioactiveResource.Split('.')[1]}\n";
            }

            //Use Mass Dismantle Cheat's crate creation function to package the items into a crate entity
            MassDismantleCheat.CreateCrateEntityFromInventory(rootItem, inventory, saveGame.Header.BuildVersion);

            //MessageBox.Show("Player name " + playerEntityModel.Title);
            MessageBox.Show($"Crate created.\nNote that normally unstackable items will visually display as being stacked to 1. Use Ctrl+Click to transfer items out of the crate without deleting part of the stack.\n\nSkipped the following items marked as radioactive:\n\n{skipped}", $"Processed {resourceStrings.Count} resource paths");

            //Ask the player if they'd like the be equipped with a hazmat suit and filters since the box will be radioactive
            /*MessageBoxResult result = MessageBox.Show("A crate with all resources has been generated at your feet. This includes radioactive items. Would you like a Hazmat suit and filters? They will replace your current equipment and first inventory item.", $"Processed {resourceStrings.Count} resource paths", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string hazmatPath = "/Game/FactoryGame/Resource/Equipment/HazmatSuit/BP_EquipmentDescriptorHazmatSuit.BP_EquipmentDescriptorHazmatSuit_C";
                string filterPath = "/Game/FactoryGame/Resource/Parts/IodineInfusedFilter/Desc_HazmatFilter.Desc_HazmatFilter_C";

                //playerEntityModel.FindOrCreateField<ObjectProperty>("mInventory");
                var playerName = playerEntityModel.Title;
                var playerBackSlot = rootItem.FindChild(playerName + ".BackSlot", true);
                if (playerBackSlot == null)
                {
                    MessageBox.Show("Your player does not seem to have an associated back slot.\nThe crate has still been created, but you have not been given Hazmat equipment.", "Cannot find host player inventory.", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //MessageBox.Show(playerBackSlot.FindOrCreateField<ArrayPropertyViewModel>("mInventoryStacks").Elements[0]);
                    MessageBox.Show("Currently unimplemented");
                }
            }*/

            return true;
        }
    }
}
