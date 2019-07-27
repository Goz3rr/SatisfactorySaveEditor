using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGPowerInfoComponent")]
    public class FGPowerInfoComponent : SaveComponent
    {
        [SaveProperty("mTargetConsumption")]
        public float TargetConsumption { get; set; }

        [SaveProperty("mBaseProduction")]
        public float BaseProduction { get; set; }

        [SaveProperty("mDynamicProductionCapacity")]
        public float DynamicProductionCapacity { get; set; }
    }
}
