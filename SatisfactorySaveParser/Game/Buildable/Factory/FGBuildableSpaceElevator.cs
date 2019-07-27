using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/SpaceElevator/Build_SpaceElevator.Build_SpaceElevator_C")]
    public class FGBuildableSpaceElevator : FGBuildableFactory
    {
        [SaveProperty("mInputInventory")]
        public ObjectReference InputInventory { get; set; }
    }
}
