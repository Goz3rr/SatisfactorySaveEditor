using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Vehicle/Golfcart/BP_Golfcart.BP_Golfcart_C")]
    public class Golfcart : FGWheeledVehicle
    {
        [SaveProperty("mIsFoldAnimationDone")]
        public bool IsFoldAnimationDone { get; set; }
    }
}
