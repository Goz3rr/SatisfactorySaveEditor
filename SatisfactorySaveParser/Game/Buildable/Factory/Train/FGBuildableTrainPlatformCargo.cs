using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory.Train
{
    public abstract class FGBuildableTrainPlatformCargo : FGBuildableTrainPlatform
    {
        [SaveProperty("mInventory")]
        public ObjectReference Inventory { get; set; }

        [SaveProperty("mHasDockedActor")]
        public bool HasDockedActor { get; set; }

        [SaveProperty("mIsInLoadMode")]
        public bool IsInLoadMode { get; set; }

        [SaveProperty("mIsLoadUnloading")]
        public bool IsLoadUnloading { get; set; }

        [SaveProperty("mShouldExecuteLoadOrUnload")]
        public bool ShouldExecuteLoadOrUnload { get; set; }

        [SaveProperty("mTrackPosition")]
        public FRailroadTrackPosition TrackPosition { get; set; }
    }
}
