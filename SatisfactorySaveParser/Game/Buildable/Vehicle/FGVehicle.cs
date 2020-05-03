using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Vehicle
{
    public abstract class FGVehicle : SaveActor
    {
        [SaveProperty("mHealthComponent")]
        public ObjectReference HealthComponent { get; set; }

        /// <summary>
        ///     Recipe this building was built with, e.g. used for refunds and stats.
        /// </summary>
        [SaveProperty("mBuiltWithRecipe")]
        public ObjectReference BuiltWithRecipe { get; set; }

        [SaveProperty("mDismantleRefund")]
        public List<FItemAmount> DismantleRefund { get; } = new List<FItemAmount>();

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
    }
}
