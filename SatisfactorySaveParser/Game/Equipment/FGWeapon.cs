using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    public abstract class FGWeapon : FGEquipment
    {
        [SaveProperty("mCurrentAmmo")]
        public int CurrentAmmo { get; set; }
    }
}
