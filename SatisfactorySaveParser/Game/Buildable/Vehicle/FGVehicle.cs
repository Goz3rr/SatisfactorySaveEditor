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

        [SaveProperty("mDismantleRefund")]
        public List<FItemAmount> DismantleRefund { get; } = new List<FItemAmount>();

        [SaveProperty("mPrimaryColor")]
        public FLinearColor PrimaryColor { get; set; }

        [SaveProperty("mSecondaryColor")]
        public FLinearColor SecondaryColor { get; set; }
    }
}
