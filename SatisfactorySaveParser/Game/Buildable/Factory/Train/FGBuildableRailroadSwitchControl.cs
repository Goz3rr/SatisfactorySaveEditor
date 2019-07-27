using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/Train/SwitchControl/Build_RailroadSwitchControl.Build_RailroadSwitchControl_C")]
    public class FGBuildableRailroadSwitchControl : FGBuildableFactory
    {
        [SaveProperty("mControlledConnection")]
        public ObjectReference ControlledConnection { get; set; }
    }
}
