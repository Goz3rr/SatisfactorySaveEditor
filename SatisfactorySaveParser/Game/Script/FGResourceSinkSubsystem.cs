using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     Subsystem to handle the resource sink and the rewards from sinked items
    /// </summary>
    [SaveObjectClass("/Script/FactoryGame.FGResourceSinkSubsystem")]
    public class FGResourceSinkSubsystem : SaveActor
    {
        /// <summary>
        ///     The total number of resource sink points we have accumulated in total
        /// </summary>
        [SaveProperty("mTotalResourceSinkPoints")]
        public long TotalResourceSinkPoints { get; set; }

        /// <summary>
        ///     The current point level we have reached, this value only increases and isn't not affected by printing coupons
        /// </summary>
        [SaveProperty("mCurrentPointLevel")]
        public int CurrentPointLevel { get; set; }

        /// <summary>
        ///     The number of coupons we have to our disposal to print and use
        /// </summary>
        [SaveProperty("mNumResourceSinkCoupons")]
        public int NumResourceSinkCoupons { get; set; }

        /// <summary>
        ///     The data for the global points history of the resource sink subsystem
        /// </summary>
        [SaveProperty("mGlobalPointHistory")]
        public List<int> GlobalPointHistory { get; } = new List<int>();

        /// <summary>
        ///     The items that the player tried to sink that you can't sink that is also present in mFailedItemSinkMessages
        /// </summary>
        [SaveProperty("mItemsFailedToSink")]
        public List<ObjectReference> ItemsFailedToSink { get; } = new List<ObjectReference>();

        /// <summary>
        ///     Have we ever tried to sink any item that you can't sink that is not present in mFailedItemSinkMessages
        /// </summary>
        [SaveProperty("mAnyGenericItemsFailedToSink")]
        public bool AnyGenericItemsFailedToSink { get; set; }

        /// <summary>
        ///     Have we sunken a item of the coupon class, Used to give a schematic
        /// </summary>
        [SaveProperty("mIsCouponEverSunk")]
        public bool IsCouponEverSunk { get; set; }
    }
}
