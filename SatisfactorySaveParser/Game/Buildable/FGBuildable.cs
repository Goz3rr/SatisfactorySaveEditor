using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

// Buildables\FGBuildable.h

namespace SatisfactorySaveParser.Game.Buildable
{
    /// <summary>
    ///     Base for everything buildable, buildable things can have factory connections, power connections etc.
    /// </summary>
    public class FGBuildable : SaveActor
    {
        /// <summary>
        ///     The primary color of this buildable
        /// </summary>
        [SaveProperty("mPrimaryColor")]
        public FLinearColor PrimaryColor { get; set; }

        /// <summary>
        ///     The primary color of this buildable
        /// </summary>
        [SaveProperty("mSecondaryColor")]
        public FLinearColor SecondaryColor { get; set; }

        /// <summary>
        ///     The color slot of this buildable
        /// </summary>
        [SaveProperty("mColorSlot")]
        public byte ColorSlot { get; set; }

        /// <summary>
        ///     If this building should show highlight before first use, save when it has been shown
        /// </summary>
        [SaveProperty("mDidFirstTimeUse")]
        public bool DidFirstTimeUse { get; set; }

        /// <summary>
        ///     The building ID this belongs to.
        /// </summary>
        [SaveProperty("mBuildingID")]
        public int BuildingID { get; set; }

        [SaveProperty("mDismantleRefund")]
        public List<FItemAmount> DismantleRefund { get; } = new List<FItemAmount>();

        /// <summary>
        ///     Recipe this building was built with, e.g. used for refunds and stats.
        /// </summary>
        [SaveProperty("mBuiltWithRecipe")]
        public ObjectReference BuiltWithRecipe { get; set; }

        /// <summary>
        ///     Time when this building was built
        /// </summary>
        [SaveProperty("mBuildTimeStamp")]
        public float BuildTimeStamp { get; set; }
    }
}
