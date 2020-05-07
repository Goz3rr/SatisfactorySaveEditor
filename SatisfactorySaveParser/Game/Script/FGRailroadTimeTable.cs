using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGRailroadTimeTable")]
    public class FGRailroadTimeTable : SaveActor
    {
        [SaveProperty("mStops")]
        public List<FTimeTableStop> Stops { get; } = new List<FTimeTableStop>();

        [SaveProperty("mCurrentStop")]
        public int CurrentStop { get; set; }
    }
}
