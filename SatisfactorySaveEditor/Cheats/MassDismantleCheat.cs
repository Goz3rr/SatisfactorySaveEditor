using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser;
using SatisfactorySaveParser.PropertyTypes;
using SatisfactorySaveParser.PropertyTypes.Structs;
using SatisfactorySaveParser.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class MassDismantleCheat : ICheat
    {
        public string Name => "Mass dismantle...";

        private int GetNextStorageID(int currentId, SaveObjectModel rootItem)
        {
            while (rootItem.FindChild($"Persistent_Level:PersistentLevel.BP_Crate_C_{currentId}.inventory", false) != null)
                currentId++;
            return currentId;
        }

        public bool IsPointInPolygon(Vector3 p, Vector3[] polygon)
        {
            float minX = polygon[0].X;
            float maxX = polygon[0].X;
            float minY = polygon[0].Y;
            float maxY = polygon[0].Y;
            for (int i = 1; i < polygon.Length; i++)
            {
                Vector3 q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
                return false;

            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                     p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private Vector3[] polygon;
        private float minZ, maxZ;

        private void BuildPolygon()
        {
            List<Vector3> points = new List<Vector3>();
            bool done = false;
            while (!done)
            {
                MassDismantleWindow massDeleteWindow = new MassDismantleWindow();
                if (!massDeleteWindow.ShowDialog().Value)
                    break;
                if (!massDeleteWindow.ResultSet)
                    break;
                points.Add(massDeleteWindow.Result);
                if (massDeleteWindow.Done)
                    done = true;
            }
            polygon = points.ToArray();
            MassDismantleWindow zWindow = new MassDismantleWindow(isZWindow: true);
            if (!zWindow.ShowDialog().Value || !zWindow.ResultSet)
            {
                minZ = float.NegativeInfinity;
                maxZ = float.PositiveInfinity;
            }
            else
            {
                minZ = zWindow.Result.X;
                maxZ = zWindow.Result.Y;
            }
        }

        public byte[] PrepareForParse(string itemPath, int itemAmount)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.WriteLengthPrefixedString("mInventoryStacks");
                    writer.WriteLengthPrefixedString("StructProperty");
                    writer.Write("Item".GetSerializedLength() + "StructProperty".GetSerializedLength() + 4 + 4 + "InventoryItem".GetSerializedLength() + 4 + 4 + 4 + 4 + 1 + 4 + itemPath.GetSerializedLength() + "".GetSerializedLength() + "".GetSerializedLength() + "NumItems".GetSerializedLength() + "IntProperty".GetSerializedLength() + 4 + 4 + 1 + 4 + "None".GetSerializedLength()); // TODO
                    writer.Write(0);
                    writer.WriteLengthPrefixedString("InventoryStack");
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write((byte)0);
                    writer.WriteLengthPrefixedString("Item");
                    writer.WriteLengthPrefixedString("StructProperty");
                    writer.Write(4 + itemPath.GetSerializedLength() + "".GetSerializedLength() + "".GetSerializedLength());
                    writer.Write(0);
                    writer.WriteLengthPrefixedString("InventoryItem"); // ItemType
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write((byte)0);
                    writer.Write(0); // Unknown1
                    writer.WriteLengthPrefixedString(itemPath); // ItemType
                    writer.WriteLengthPrefixedString(""); // Unknown2
                    writer.WriteLengthPrefixedString(""); // Unknown3
                    writer.WriteLengthPrefixedString("NumItems");
                    writer.WriteLengthPrefixedString("IntProperty");
                    writer.Write(4); // Length
                    writer.Write(0); // Index
                    writer.Write((byte)0);
                    writer.Write(itemAmount); // Value
                    writer.WriteLengthPrefixedString("None");

                }
                return ms.ToArray();
            }
        }

        private int MassDismantle(List<SaveObjectModel> objects, ArrayProperty inventory, SaveObjectModel rootItem)
        {
            int count = 0;
            foreach (SaveObjectModel item in objects)
            {
                if (item is SaveEntityModel)
                    if (IsPointInPolygon(((SaveEntityModel)item).Position, polygon) && minZ <= ((SaveEntityModel)item).Position.Z && ((SaveEntityModel)item).Position.Z <= maxZ)
                    {
                        ArrayPropertyViewModel dismantleRefund = ((SaveEntityModel)item).FindField<ArrayPropertyViewModel>("mDismantleRefund");
                        if (dismantleRefund != null)
                        {
                            foreach (SerializedPropertyViewModel property in dismantleRefund.Elements)
                            {
                                DynamicStructData itemAmountStruct = (DynamicStructData)((StructProperty)property.Model).Data;
                                string itemPath = ((ObjectProperty)itemAmountStruct.Fields[0]).PathName;
                                int itemAmount = ((IntProperty)itemAmountStruct.Fields[1]).Value;
                                byte[] bytes = PrepareForParse(itemPath, itemAmount);
                                using (MemoryStream ms = new MemoryStream(bytes))
                                using (BinaryReader reader = new BinaryReader(ms))
                                {
                                    SerializedProperty prop = SerializedProperty.Parse(reader);
                                    inventory.Elements.Add(prop);
                                }
                            }
                        }
                        if (item.FindField<ObjectPropertyViewModel>("mInventory") != null)
                            inventory.Elements.AddRange(((ArrayProperty)rootItem.FindChild(item.FindField<ObjectPropertyViewModel>("mInventory").Str2, false).FindField<ArrayPropertyViewModel>("mInventoryStacks").Model).Elements);
                        if (item.FindField<ObjectPropertyViewModel>("mStorageInventory") != null)
                            inventory.Elements.AddRange(((ArrayProperty)rootItem.FindChild(item.FindField<ObjectPropertyViewModel>("mStorageInventory").Str2, false).FindField<ArrayPropertyViewModel>("mInventoryStacks").Model).Elements);
                        if (item.FindField<ObjectPropertyViewModel>("mInputInventory") != null)
                            inventory.Elements.AddRange(((ArrayProperty)rootItem.FindChild(item.FindField<ObjectPropertyViewModel>("mInputInventory").Str2, false).FindField<ArrayPropertyViewModel>("mInventoryStacks").Model).Elements);
                        if (item.FindField<ObjectPropertyViewModel>("mOutputInventory") != null)
                            inventory.Elements.AddRange(((ArrayProperty)rootItem.FindChild(item.FindField<ObjectPropertyViewModel>("mOutputInventory").Str2, false).FindField<ArrayPropertyViewModel>("mInventoryStacks").Model).Elements);
                        rootItem.Remove(item);
                        count++;
                    }
            }
            return count;
        }

        public bool Apply(SaveObjectModel rootItem)
        {
            BuildPolygon();
            if (polygon.Length < 2)
            {
                MessageBox.Show("At least 2 points needed to mass delete", "Could not mass delete", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            ArrayProperty inventory = new ArrayProperty("mInventoryStacks")
            {
                Type = "StructProperty"
            };
            int countFactory = 0, countBuilding = 0, countCrate = 0;
            try
            {
                countFactory = MassDismantle(rootItem.FindChild("Buildable", true).FindChild("Factory", true).DescendantSelfViewModel, inventory, rootItem);
            }
            catch (NullReferenceException) { }
            try
            {
                countBuilding = MassDismantle(rootItem.FindChild("Buildable", true).FindChild("Building", true).DescendantSelfViewModel, inventory, rootItem);
            }
            catch (NullReferenceException) { }
            try
            {
                countCrate = MassDismantle(rootItem.FindChild("-Shared", true).FindChild("BP_Crate.BP_Crate_C", true).DescendantSelfViewModel, inventory, rootItem);
            }
            catch (NullReferenceException) { }
            MessageBoxResult result = MessageBox.Show($"Deleted {countFactory} factory buildings, {countBuilding} foundations and {countCrate} crates. Drop the items (including items in deleted storages) in a single crate?", "Deleted", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                inventory = ArrangeInventory(inventory);
                int currentStorageID = GetNextStorageID(0, rootItem);
                SaveComponent newInventory = new SaveComponent("/Script/FactoryGame.FGInventoryComponent", "Persistent_Level", $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}.inventory")
                {
                    ParentEntityName = $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}",
                    DataFields = new SerializedFields()
                    {
                        inventory,
                        new ArrayProperty("mArbitrarySlotSizes")
                        {
                            Type = "IntProperty",
                            Elements = Enumerable.Repeat(new IntProperty("Element"){ Value = 0 }, inventory.Elements.Count).Cast<SerializedProperty>().ToList()
                        },
                        new ArrayProperty("mAllowedItemDescriptors")
                        {
                            Type = "ObjectProperty",
                            Elements = Enumerable.Repeat(new ObjectProperty("Element"){ LevelName = "", PathName = "" }, inventory.Elements.Count).Cast<SerializedProperty>().ToList()
                        }
                    }
                };
                rootItem.FindChild("FactoryGame.FGInventoryComponent", false).Items.Add(new SaveComponentModel(newInventory));
                SaveEntity player = (SaveEntity)rootItem.FindChild("Char_Player.Char_Player_C", false).DescendantSelf[0];
                SaveEntity newSaveObject = new SaveEntity("/Game/FactoryGame/-Shared/Crate/BP_Crate.BP_Crate_C", "Persistent_Level", $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}")
                {
                    NeedTransform = true,
                    Rotation = player.Rotation,
                    Position = new Vector3() { X = player.Position.X, Y = player.Position.Y + 100, Z = player.Position.Z },
                    Scale = new Vector3() { X = 1, Y = 1, Z = 1 },
                    WasPlacedInLevel = false,
                    ParentObjectName = "",
                    ParentObjectRoot = ""
                };
                newSaveObject.DataFields = new SerializedFields()
                {
                    new ObjectProperty("mInventory", 0) { LevelName = "Persistent_Level", PathName = $"Persistent_Level:PersistentLevel.BP_Crate_C_{currentStorageID}.inventory" }
                };
                if (rootItem.FindChild("Crate", false) == null)
                    rootItem.FindChild("-Shared", false).Items.Add(new SaveObjectModel("Crate"));
                if (rootItem.FindChild("BP_Crate.BP_Crate_C", false) == null)
                    rootItem.FindChild("Crate", false).Items.Add(new SaveObjectModel("BP_Crate.BP_Crate_C"));
                rootItem.FindChild("BP_Crate.BP_Crate_C", false).Items.Add(new SaveEntityModel(newSaveObject));
            }
            return true;
        }



        private ArrayProperty ArrangeInventory(ArrayProperty inventory)
        {
            SortedDictionary<string, int> stacks = new SortedDictionary<string, int>();
            foreach (StructProperty inventoryStruct in inventory.Elements.Cast<StructProperty>())
            {
                DynamicStructData inventoryStack = (DynamicStructData) inventoryStruct.Data;
                InventoryItem inventoryItem = (InventoryItem)((StructProperty)inventoryStack.Fields[0]).Data;
                IntProperty itemCount = (IntProperty)inventoryStack.Fields[1];
                if (!stacks.ContainsKey(inventoryItem.ItemType))
                    stacks[inventoryItem.ItemType] = 0;
                stacks[inventoryItem.ItemType] += itemCount.Value;
            }
            ArrayProperty newInventory = new ArrayProperty("mInventoryStacks")
            {
                Type = "StructProperty"
            };
            foreach(KeyValuePair<string, int> itemStack in stacks)
            {
                string itemPath = itemStack.Key;
                if (string.IsNullOrWhiteSpace(itemPath))
                    continue;
                int itemAmount = itemStack.Value;
                byte[] bytes = PrepareForParse(itemPath, itemAmount);
                using (MemoryStream ms = new MemoryStream(bytes))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    SerializedProperty prop = SerializedProperty.Parse(reader);
                    newInventory.Elements.Add(prop);
                }
            }
            return newInventory;
        }
    }
}
