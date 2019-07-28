using System.Collections.Generic;

using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Schematics.Progression
{
    [SaveObjectClass("/Game/FactoryGame/Schematics/Progression/BP_GamePhaseManager.BP_GamePhaseManager_C")]
    public class GamePhaseManager : SaveActor
    {
        [SaveProperty("mGamePhase")]
        public EnumAsByte<EGamePhase> GamePhase { get; set; }

        [SaveProperty("mGamePhaseCosts")]
        public List<FPhaseCost> GamePhaseCosts { get; } = new List<FPhaseCost>();
    }
}
