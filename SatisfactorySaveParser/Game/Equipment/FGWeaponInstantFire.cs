using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    public abstract class FGWeaponInstantFire : FGWeapon
    {
        [SaveProperty("mHasReloadedOnce")]
        public bool HasReloadedOnce { get; set; }
    }
}
