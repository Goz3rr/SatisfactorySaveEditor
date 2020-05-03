#pragma warning disable CA1707 // Identifiers should not contain underscores

using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Character.Player
{
    [SaveObjectClass("/Game/FactoryGame/Character/Player/Char_Player.Char_Player_C")]
    public class FGCharacterPlayer : FGCharacterBase
    {
        public const int MAX_SAFE_GROUND_POS_BUFFER_SIZE = 3;

        [SaveProperty("mBuildGun")]
        public ObjectReference BuildGun { get; set; }

        [SaveProperty("mResourceScanner")]
        public ObjectReference ResourceScanner { get; set; }

        [SaveProperty("mResourceMiner")]
        public ObjectReference ResourceMiner { get; set; }

        [SaveProperty("mLastSafeGroundPositions")]
        public FVector[] LastSafeGroundPositions { get; } = new FVector[MAX_SAFE_GROUND_POS_BUFFER_SIZE];

        [SaveProperty("mLastSafeGroundPositionLoopHead")]
        public int LastSafeGroundPositionLoopHead { get; set; }

        [SaveProperty("mInventory")]
        public ObjectReference Inventory { get; set; }

        [SaveProperty("mBeltSlot")]
        public ObjectReference BeltSlot { get; set; }

        [SaveProperty("mSavedDrivenVehicle")]
        public ObjectReference SavedDrivenVehicle { get; set; }

        [SaveProperty("mFlashlightOn")]
        public bool FlashlightOn { get; set; }
    }
}
