
using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    public class FGBuildableConveyorLift : FGBuildableConveyorBase
    {
        [SaveProperty("mTopTransform")]
        public FTransform TopTransform { get; set; }

        [SaveProperty("mIsReversed")]
        public bool IsReversed { get; set; }
    }
}
