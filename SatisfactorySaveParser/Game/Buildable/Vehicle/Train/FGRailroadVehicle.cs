using System.IO;

using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle.Train
{
    public abstract class FGRailroadVehicle : FGVehicle
    {
        [SaveProperty("mIsDockedAtPlatform")]
        public bool IsDockedAtPlatform { get; set; }

        [SaveProperty("mIsOrientationReversed")]
        public bool IsOrientationReversed { get; set; }

        [SaveProperty("mTrackPosition")]
        public FRailroadTrackPosition TrackPosition { get; set; }

        public ObjectReference NextVehicle { get; set; }
        public ObjectReference PreviousVehicle { get; set; }

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            reader.AssertNullInt32();
            NextVehicle = reader.ReadObjectReference();
            PreviousVehicle = reader.ReadObjectReference();
        }
    }
}
