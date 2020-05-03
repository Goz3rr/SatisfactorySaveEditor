using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Buildable.Factory
{
    /// <summary>
    ///     Building where you can buy schematics with coupons
    /// </summary>
    [SaveObjectClass("/Game/FactoryGame/Buildable/Factory/ResourceSinkShop/Build_ResourceSinkShop.Build_ResourceSinkShop_C")]
    public class ResourceSinkShop : FGBuildableFactory
    {
        /// <summary>
        ///     The inventory that holds the purchases we made in the resource sink shop
        /// </summary>
        [SaveProperty("mShopInventory")]
        public ObjectReference ShopInventory { get; set; }
    }
}
