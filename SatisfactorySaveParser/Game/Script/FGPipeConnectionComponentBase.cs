using SatisfactorySaveParser.Save;

// FGPipeConnectionComponent.h

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     Connection base used to link generic pipes together
    /// </summary>
    public abstract class FGPipeConnectionComponentBase : FGConnectionComponent
    {
        /// <summary>
        ///     Connection to another component. If this is set we're connected.
        /// </summary>
        [SaveProperty("mConnectedComponent")]
        public ObjectReference ConnectedComponent { get; set; }
    }
}
