using System.Collections.Generic;
using SatisfactorySaveParser.Game.Enums;

namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("PhaseCost")]
    public class FPhaseCost : GameStruct
    {
        public override string StructName => "PhaseCost";

        [StructProperty("gamePhase")]
        public EGamePhase GamePhase { get; set; }

        [StructProperty("Cost")]
        public List<FItemAmount> Cost { get; } = new List<FItemAmount>();
    }
}
