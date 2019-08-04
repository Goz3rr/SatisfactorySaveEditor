using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGMapManager")]
    public class FGMapManager : SaveActor
    {
        [SaveProperty("mFogOfWarRawData")]
        public List<byte> FogOfWarRawData { get; } = new List<byte>();
    }
}
