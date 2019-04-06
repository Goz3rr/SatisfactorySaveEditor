using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.ViewModel.Property;
using SatisfactorySaveParser.Data;
using SatisfactorySaveParser.PropertyTypes;
using System.Linq;
using System.Windows;

namespace SatisfactorySaveEditor.Cheats
{
    public class ResearchUnlockCheat : ICheat
    {
        public string Name => "Unlock all research";

        public bool Apply(SaveObjectModel rootItem)
        {
            var cheatObject = rootItem.FindChild("Persistent_Level:PersistentLevel.schematicManager", false);
            if (cheatObject == null)
            {
                MessageBox.Show("This save does not contain a schematicManager.\nThis means that the loaded save is probably corrupt. Aborting.", "Cannot find schematicManager", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            foreach (var field in cheatObject.Fields)
            {
                if (field.PropertyName == "mAvailableSchematics" || field.PropertyName == "mPurchasedSchematics")
                {
                    if (!(field is ArrayPropertyViewModel arrayField))
                    {
                        MessageBox.Show("Expected schematic data is of wrong type.\nThis means that the loaded save is probably corrupt. Aborting.", "Wrong schematics type", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                    foreach (var research in Research.GetResearches())
                    {
                        if (!arrayField.Elements.Cast<ObjectPropertyViewModel>().Any(e => e.Str2 == research.Path))
                        {
                            arrayField.Elements.Add(new ObjectPropertyViewModel(new ObjectProperty(null, "", research.Path)));
                        }
                    }
                }
            }

            MessageBox.Show("All research successfully unlocked.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
