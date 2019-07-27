using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGFactoryLegsComponent")]
    public class FGFactoryLegsComponent : SaveComponent
    {
        [SaveProperty("mCachedFeetOffset")]
        public List<FFeetOffset> CachedFeetOffset { get; } = new List<FFeetOffset>();
    }
}
