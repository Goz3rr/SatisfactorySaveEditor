using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    public abstract class FGCircuitConnectionComponent : FGConnectionComponent
    {
        [SaveProperty("mWires")]
        public List<ObjectReference> Wires { get; } = new List<ObjectReference>();

        [SaveProperty("mNbWiresConnected")]
        public byte NbWiresConnected { get; set; }

        [SaveProperty("mHiddenConnections")]
        public List<ObjectReference> HiddenConnections { get; } = new List<ObjectReference>();

        [SaveProperty("mCircuitID")]
        public int CircuitID { get; set; }
    }
}
