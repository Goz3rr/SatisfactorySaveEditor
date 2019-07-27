using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/RadarTower/Build_RadarTower.Build_RadarTower_C")]
    public class FGBuildableRadarTower : FGBuildableFactory
    {
        [SaveProperty("mCurrentExpansionStep")]
        public int CurrentExpansionStep { get; set; }

        [SaveProperty("mTimeToNextExpansion")]
        public float TimeToNextExpansion { get; set; }
    }
}
