using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Equipment
{
    [SaveObjectClass("/Game/FactoryGame/Equipment/NobeliskDetonator/Equip_NobeliskDetonator.Equip_NobeliskDetonator_C")]
    public class NobeliskDetonator : FGWeapon
    {
        [SaveProperty("mDispensedExplosives")]
        public List<ObjectReference> DispensedExplosives { get; } = new List<ObjectReference>();
    }
}
