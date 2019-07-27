using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    [SaveObjectClass("/Script/FactoryGame.FGFoliageRemoval")]
    public class FGFoliageRemoval : SaveActor
    {
        [SaveProperty("mRemovedInstances")]
        public FRemovedInstanceArray RemovedInstances { get; set; }

        [SaveProperty("mLevelName")]
        public string LevelName { get; set; }

        [SaveProperty("mFoliageTypeName")]
        public string FoliageTypeName { get; set; }

        [SaveProperty("mLevelBounds")]
        public Box LevelBounds { get; set; }
    }
}
