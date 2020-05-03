using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Unlocks
{
    /// <summary>
    ///     Subsystem responsible for handling unlocks that you get when purchasing/research a schematic
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/Unlocks/BP_UnlockSubsystem.BP_UnlockSubsystem_C")]
    public class UnlockSubsystem : SaveActor
    {
        /// <summary>
        ///     These are the resources the players can use their scanner to find 
        /// </summary>
        [SaveProperty("mScannableResources")]
        public List<ObjectReference> ScannableResources { get; } = new List<ObjectReference>();

        /// <summary>
        ///     Did the player unlock the minimap?
        /// </summary>
        [SaveProperty("mIsMapUnlocked")]
        public bool IsMapUnlocked { get; set; }

        /// <summary>
        ///     Is the building efficiency display unlocked 
        /// </summary>
        [SaveProperty("mIsBuildingEfficiencyUnlocked")]
        public bool IsBuildingEfficiencyUnlocked { get; set; }

        /// <summary>
        ///     Is the building overclocking unlocked
        /// </summary>
        [SaveProperty("mIsBuildingOverclockUnlocked")]
        public bool IsBuildingOverclockUnlocked { get; set; }

        /// <summary>
        ///     The highest total number of inventory slots that any player have ever had, saved for save compatibility and rebalancing
        /// </summary>
        [SaveProperty("mNumTotalInventorySlots")]
        public int NumTotalInventorySlots { get; set; }

        /// <summary>
        ///     The highest total number of arm equipment slots that any player have ever had, saved for save compatibility and rebalancing
        /// </summary>
        [SaveProperty("mNumTotalArmEquipmentSlots")]
        public int NumTotalArmEquipmentSlots { get; set; }
    }
}
