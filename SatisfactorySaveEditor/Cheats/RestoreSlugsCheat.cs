
using SatisfactorySaveEditor.Model;

using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Cheats
{
    public class RestoreSlugsCheat : ICheat
    {
        public string Name => "Restore slugs";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            foreach (var level in saveGame.Levels)
            {
                level.CollectedObjects.RemoveAll(x => x.PathName.Contains("PersistentLevel.BP_Crystal"));
            }
            
            return true;
        }
    }
}
