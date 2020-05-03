using SatisfactorySaveParser.Game.Enums;
using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     This component is used on factories to connect to.
    /// </summary>
    [SaveObjectClass("/Script/FactoryGame.FGFactoryConnectionComponent")]
    public class FGFactoryConnectionComponent : FGConnectionComponent
    {
        /// <summary>
        ///     Direction for this connection.
        /// </summary>
        [SaveProperty("mDirection")]
        public EFactoryConnectionDirection Direction { get; set; }

        /// <summary>
        ///     Connection to another component. If this is set we're connected.
        /// </summary>
        [SaveProperty("mConnectedComponent")]
        public ObjectReference ConnectedComponent { get; set; }

        /// <summary>
        ///     The inventory of this connection
        /// </summary>
        [SaveProperty("mConnectionInventory")]
        public ObjectReference ConnectionInventory { get; set; }

        /// <summary>
        ///     The inventory index utilized by this connection ( -1 for none specified )
        /// </summary>
        [SaveProperty("mInventoryAccessIndex")]
        public int InventoryAccessIndex { get; set; }
    }
}
