using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Building that you can sink items into to get points
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/ResourceSink/Build_ResourceSink.Build_ResourceSink_C")]
    public class ResourceSink : FGBuildableFactory
    {
        [SaveProperty("mCouponInventory")]
        public ObjectReference CouponInventory { get; set; }
    }
}
