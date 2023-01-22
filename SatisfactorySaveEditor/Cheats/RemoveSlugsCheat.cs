using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var slugTypes = new[] {
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal.BP_Crystal_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk2.BP_Crystal_mk2_C",
                "/Game/FactoryGame/Resource/Environment/Crystal/BP_Crystal_mk3.BP_Crystal_mk3_C"
            };

            foreach (var level in saveGame.Levels)
            {

                var slugs = level.Entries.Where(x => slugTypes.Contains(x.TypePath));

                level.CollectedObjects.RemoveAll(x => x.PathName.Contains("PersistentLevel.BP_Crystal"));
                level.CollectedObjects.AddRange(slugs.Select(s => new ObjectReference
                {
                    LevelName = s.RootObject,
                    PathName = s.InstanceName
                }));
            }

            return true;
        }
    }
}
