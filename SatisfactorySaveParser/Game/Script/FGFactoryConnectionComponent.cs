using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGFactoryConnectionComponent")]
    public class FGFactoryConnectionComponent : FGConnectionComponent
    {
        [SaveProperty("mDirection")]
        public EFactoryConnectionDirection Direction { get; set; }

        [SaveProperty("mConnectedComponent")]
        public ObjectReference ConnectedComponent { get; set; }

        [SaveProperty("mConnectionInventory")]
        public ObjectReference ConnectionInventory { get; set; }
    }
}
