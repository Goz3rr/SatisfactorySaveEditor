using System.Collections.Generic;
using System.IO;
using System.Numerics;

using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle
{
    public abstract class FGWheeledVehicle : FGVehicle
    {
        [SaveProperty("mCurrentFuelAmount")]
        public float CurrentFuelAmount { get; set; }

        [SaveProperty("mIsLoadingVehicle")]
        public bool IsLoadingVehicle { get; set; }

        [SaveProperty("mIsUnloadingVehicle")]
        public bool IsUnloadingVehicle { get; set; }

        [SaveProperty("mCurrentFuelClass")]
        public ObjectReference CurrentFuelClass { get; set; }

        [SaveProperty("mIsSimulated")]
        public bool IsSimulated { get; set; }

        [SaveProperty("mFuelInventory")]
        public ObjectReference FuelInventory { get; set; }

        [SaveProperty("mStorageInventory")]
        public ObjectReference StorageInventory { get; set; }

        [SaveProperty("mTargetNodeLinkedList")]
        public ObjectReference TargetNodeLinkedList { get; set; }

        [SaveProperty("mIsPathVisible")]
        public bool IsPathVisible { get; set; }

        [SaveProperty("mCurrentDestination")]
        public FVector CurrentDestination { get; set; }

        [SaveProperty("mDesiredSteering")]
        public float DesiredSteering { get; set; }

        [SaveProperty("mSpeedLimit")]
        public int SpeedLimit { get; set; }

        [SaveProperty("mIsAutoPilotEnabled")]
        public bool IsAutoPilotEnabled { get; set; }

        public List<MaybeBone> MaybeBones { get; } = new List<MaybeBone>();

        public override void DeserializeNativeData(BinaryReader reader, int length)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                MaybeBones.Add(new MaybeBone()
                {
                    Name = reader.ReadLengthPrefixedString(),
                    Position = reader.ReadVector3(),
                    Rotation = reader.ReadVector4(),
                    Unknown = reader.ReadBytes(25)
                });
            }
        }
    }

    public class MaybeBone
    {
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public Vector4 Rotation { get; set; }
        public byte[] Unknown { get; set; }
    }
}
