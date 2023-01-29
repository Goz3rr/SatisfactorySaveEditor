using System.Linq;
using System.Windows;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveParser;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveEditor.Cheats
{
    public class RemoveSlugsCheat : ICheat
    {
        public string Name => "Remove slugs";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            MessageBox.Show("Can only remove already discovered slugs on the map. Continue?", "Remove slugs?", MessageBoxButton.YesNo);
            
            var slugTypes = new[] {
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal.BP_Crystal_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk2.BP_Crystal_mk2_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk3.BP_Crystal_mk3_C"
            };
            
            var slugsMk1 = rootItem.FindChild("BP_Crystal.BP_Crystal_C", false);
            var slugsMk2 = rootItem.FindChild("BP_Crystal_mk2.BP_Crystal_mk2_C", false);
            var slugsMk3 = rootItem.FindChild("BP_Crystal_mk3.BP_Crystal_mk3_C", false);

            foreach (SaveLevel level in saveGame.Levels)
            {
                var slugsForLevel = level.Entries.Where(x => slugTypes.Contains(x.TypePath));
                var readyToCollectSlugs = slugsForLevel.Where(slug => !slug.RootObject.Equals("Persistent_Exploration_2"));

                level.CollectedObjects.AddRange(readyToCollectSlugs.Select(s => new ObjectReference
                {
                    LevelName = s.RootObject,
                    PathName = s.InstanceName
                }));
            }
            
            slugsMk1.Items.Clear();
            slugsMk2.Items.Clear();
            slugsMk3.Items.Clear();

            MessageBox.Show("Removed discovered slugs on the map.", "Removed slugs.", MessageBoxButton.OK);
            return true;
        }
    }
}
