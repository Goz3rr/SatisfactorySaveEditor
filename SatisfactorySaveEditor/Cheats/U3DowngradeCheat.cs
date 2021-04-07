
using SatisfactorySaveEditor.Model;

using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Cheats
{
    public class U3DowngradeCheat : ICheat
    {
        public string Name => "Downgrade to Update 3";

        public bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame)
        {
            saveGame.Header.HeaderVersion = SatisfactorySaveParser.Save.SaveHeaderVersion.LookAtTheComment;

            var header = rootItem as SaveRootModel;
            header.BuildVersion = 140083;

            return true;
        }
    }
}
