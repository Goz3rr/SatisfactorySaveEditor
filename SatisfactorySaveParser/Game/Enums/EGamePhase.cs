#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1069 // Enums values should not be duplicated

using System;

namespace SatisfactorySaveParser.Game.Enums
{
    public enum EGamePhase
    {
        EGP_EarlyGame = 0,
        EGP_MidGame = 1,
        EGP_LateGame = 2,
        EGP_EndGame = 3,
        EGP_FoodCourt = 4,
        [Obsolete("EGP_LaunchTowTruck has been replaced by EGP_FoodCourt")]
        EGP_LaunchTowTruck = 4,
        EGP_Victory = 5
    }
}
