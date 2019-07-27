using System.Collections.Generic;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Game.Structs.Native;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable
{
    public class FGBuildable : SaveActor
    {
        [SaveProperty("mPrimaryColor")]
        public FLinearColor PrimaryColor { get; set; }

        [SaveProperty("mSecondaryColor")]
        public FLinearColor SecondaryColor { get; set; }

        [SaveProperty("mColorSlot")]
        public byte ColorSlot { get; set; }

        [SaveProperty("mDidFirstTimeUse")]
        public bool DidFirstTimeUse { get; set; }

        [SaveProperty("mBuildingID")]
        public int BuildingID { get; set; }

        [SaveProperty("mDismantleRefund")]
        public List<FItemAmount> DismantleRefund { get; } = new List<FItemAmount>();

        [SaveProperty("mBuiltWithRecipe")]
        public ObjectReference BuiltWithRecipe { get; set; }

        [SaveProperty("mBuildTimeStamp")]
        public float mBuildTimeStamp { get; set; }
    }
}
