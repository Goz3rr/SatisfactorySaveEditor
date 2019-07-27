using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    public abstract class FGCircuit : SaveComponent
    {
        [SaveProperty("mCircuitID")]
        public int CircuitID { get; set; }

        [SaveProperty("mComponents")]
        public List<ObjectReference> Components { get; } = new List<ObjectReference>();
    }
}
