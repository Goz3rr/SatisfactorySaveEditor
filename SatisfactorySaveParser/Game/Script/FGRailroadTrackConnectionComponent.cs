using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGRailroadTrackConnectionComponent")]
    public class FGRailroadTrackConnectionComponent : FGConnectionComponent
    {
        [SaveProperty("mConnectedComponents")]
        public List<ObjectReference> ConnectedComponents { get; } = new List<ObjectReference>();

        [SaveProperty("mSwitchPosition")]
        public int SwitchPosition { get; set; }
    }
}
