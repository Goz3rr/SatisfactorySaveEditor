using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/Chainsaw/Equip_Chainsaw.Equip_Chainsaw_C")]
    public class Chainsaw : FGEquipment
    {
        [SaveProperty("mEnergyStored")]
        public float EnergyStored { get; set; }
    }
}
