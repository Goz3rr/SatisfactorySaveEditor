using SatisfactorySaveParser.Save;

// FGPipeConnectionComponent.h

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     Connection used to link pipelines together
    /// </summary>
    [SaveObjectClass("/Script/FactoryGame.FGPipeConnectionComponent")]
    public class FGPipeConnectionComponent : FGPipeConnectionComponentBase
    {
        /// <summary>
        ///     The inventory of this connection. This can be null in many cases.
        /// </summary>
        [SaveProperty("mConnectionInventory")]
        public ObjectReference ConnectionInventory { get; set; }

        /// <summary>
        ///     The inventory index utilized by this connection ( -1 for none specified ).
        /// </summary>
        [SaveProperty("mInventoryAccessIndex")]
        public int InventoryAccessIndex { get; set; }

        /// <summary>
        ///     The network this connection is connected to. INDEX_NONE if not connected.
        /// </summary>
        [SaveProperty("mPipeNetworkID")]
        public int PipeNetworkID { get; set; }
    }
}
