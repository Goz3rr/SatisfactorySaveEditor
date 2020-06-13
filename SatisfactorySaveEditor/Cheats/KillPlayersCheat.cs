using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using SatisfactorySaveParser.Structures;
using System.Numerics;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class KillPlayersCheat : ICheat
    {
        public string Name => "Kill dummy players";

        private int GetNextStorageID(int currentId, SaveObjectModel rootItem)
        {
            while (rootItem.FindChild($"Persistent_Level:PersistentLevel.BP_Crate_C_{currentId}.inventory", false) != null)
                currentId++;
            return currentId;
        }

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            var players = rootItem.FindChild("Char_Player.Char_Player_C", false);
            if (players == null)
            {
                MessageBox.Show("This save does not contain a Player.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find Player", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int currentStorageID = 0;
            foreach (SaveObjectModel player in players.DescendantSelfViewModel)
            {
                string inventoryPath = player.FindField<ObjectPropertyViewModel>("mInventory").Str2;
                SaveObjectModel inventoryState = rootItem.FindChild(inventoryPath, false);
                SaveComponent inventoryComponent = (SaveComponent)inventoryState.Model;
                if (!InventoryEmpty(inventoryComponent))
                {
                    currentStorageID = GetNextStorageID(currentStorageID, rootItem);
                    SaveComponent newInventory = new SaveComponent(inventoryComponent.TypePath, inventoryComponent.RootObject, $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}.inventory")
                    {
                        ParentEntityName = $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}",
                        DataFields = inventoryComponent.DataFields
                    };
                    rootItem.FindChild("FactoryGame.FGInventoryComponent", false).Items.Add(new SaveComponentModel(newInventory));
                    SaveEntity newSaveObject = new SaveEntity("/Game/FactoryGame/-Shared/Crate/BP_Crate.BP_Crate_C", "Persistent_Level", $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}")
                    {
                        NeedTransform = true,
                        Rotation = ((SaveEntity)player.Model).Rotation,
                        Position = ((SaveEntity)player.Model).Position,
                        Scale = new Vector3() { X = 1, Y = 1, Z = 1 },
                        WasPlacedInLevel = false,
                        ParentObjectName = "",
                        ParentObjectRoot = ""
                    };
                    newSaveObject.DataFields = new SerializedFields()
                    {
                        TrailingData = null
                    };
                    newSaveObject.DataFields.Add(new ObjectProperty("mInventory", 0) { LevelName = "Persistent_Level", PathName = $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}.inventory" });
                    if (rootItem.FindChild("Crate", false) == null)
                        rootItem.FindChild("-Shared", false).Items.Add(new SaveObjectModel("Crate"));
                    if (rootItem.FindChild("BP_Crate.BP_Crate_C", false) == null)
                        rootItem.FindChild("Crate", false).Items.Add(new SaveObjectModel("BP_Crate.BP_Crate_C"));
                    rootItem.FindChild("BP_Crate.BP_Crate_C", false).Items.Add(new SaveEntityModel(newSaveObject));
                }
                rootItem.Remove(player);
                rootItem.Remove(inventoryState);
                MessageBox.Show($"Killed {player.Title}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return true;
        }

        private bool InventoryEmpty(SaveComponent inventoryComponent)
        {
            for (int i = 0; i < inventoryComponent.DataFields.Count; i++)
            {
                if (inventoryComponent.DataFields[i].PropertyName == "mInventoryStacks")
                {
                    ArrayProperty inventoryArray = (ArrayProperty)inventoryComponent.DataFields[i];
                    for (int j = 0; j < ((ArrayProperty)inventoryComponent.DataFields[i]).Elements.Count; j++)
                    {
                        StructProperty inventoryStruct = (StructProperty)inventoryArray.Elements[j];
                        DynamicStructData inventoryItem = (DynamicStructData)inventoryStruct.Data;
                        for (int k = 0; k < inventoryItem.Fields.Count; k++)
                            if (inventoryItem.Fields[k].PropertyName == "NumItems")
                            {
                                IntProperty itemCount = (IntProperty)inventoryItem.Fields[k];
                                if (itemCount.Value != 0)
                                    return false;
                            }
                    }
                    return true;
                }
            }
            return true;
        }
    }
}
