using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/ColorGun/Equip_ColorGun.Equip_ColorGun_C")]
    public class FGColorGun : FGWeaponInstantFire
    {
        [SaveProperty("mColorSlot")]
        public byte ColorSlot { get; set; }
    }
}
