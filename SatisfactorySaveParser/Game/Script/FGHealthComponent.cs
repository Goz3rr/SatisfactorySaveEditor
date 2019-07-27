using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGHealthComponent")]
    public class FGHealthComponent : SaveComponent
    {
        [SaveProperty("mMaxHealth")]
        public float MaxHealth { get; set; }

        [SaveProperty("mCurrentHealth")]
        public float CurrentHealth { get; set; }

        [SaveProperty("mRespawnHealthFactor")]
        public float RespawnHealthFactor { get; set; }

        [SaveProperty("mIsDead")]
        public bool IsDead { get; set; }
    }
}
