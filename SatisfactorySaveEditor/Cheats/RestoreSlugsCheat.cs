
using SatisfactorySaveEditor.Model;

using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Cheats
{
    public class RestoreSlugsCheat : ICheat
    {
        public string Name => "Restore slugs";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            saveGame.CollectedObjects.RemoveAll(x => x.PathName.Contains("PersistentLevel.BP_Crystal"));
            return true;
        }
    }
}
