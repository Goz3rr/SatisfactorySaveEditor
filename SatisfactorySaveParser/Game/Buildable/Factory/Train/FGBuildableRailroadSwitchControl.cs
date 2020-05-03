using SatisfactorySaveParser.Save;

// Buildables\FGBuildableRailroadSwitchControl.h

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    /// <summary>
    ///     A component for controlling a switch's position.
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/Train/SwitchControl/Build_RailroadSwitchControl.Build_RailroadSwitchControl_C")]
    public class FGBuildableRailroadSwitchControl : FGBuildableFactory
    {
        /// <summary>
        ///     Connection we control.
        /// </summary>
        [SaveProperty("mControlledConnection")]
        public ObjectReference ControlledConnection { get; set; }
    }
}
