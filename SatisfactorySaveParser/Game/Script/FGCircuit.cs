using System.Collections.Generic;

using SatisfactorySaveParser.Save;

// FGCircuit.h

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     Abstract base for circuit implementations.
    /// </summary>
    public abstract class FGCircuit : SaveComponent
    {
        /// <summary>
        ///     The id used to identify this circuit.
        /// </summary>
        [SaveProperty("mCircuitID")]
        public int CircuitID { get; set; }

        /// <summary>
        ///     List of all the components (nodes) in this circuit.
        /// </summary>
        [SaveProperty("mComponents")]
        public List<ObjectReference> Components { get; } = new List<ObjectReference>();
    }
}
