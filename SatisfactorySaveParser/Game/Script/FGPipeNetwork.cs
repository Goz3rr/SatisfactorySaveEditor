using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Script
{
    /// <summary>
    ///     A pipe network is responsible for a group of connected pipes and their fluid simulations.
    /// </summary>
    [SaveObjectClass("/Script/FactoryGame.FGPipeNetwork")]
    public class FGPipeNetwork : SaveActor
    {
        /// <summary>
        ///     Unique id of this network, assigned at spawn.
        ///     Note: This is not persistent between play sessions.
        /// </summary>
        [SaveProperty("mPipeNetworkID")]
        public int PipeNetworkID { get; set; }

        /// <summary>
        ///     The type of liquid in this network.
        /// </summary>
        [SaveProperty("mFluidDescriptor")]
        public ObjectReference FluidDescriptor { get; set; }

        /// <summary>
        ///     Compiled during save, and Interface classes are extracted on load. Stores the mFluidIntegrants in a UPROPERTY type
        /// </summary>
        [SaveProperty("mFluidIntegrantScriptInterfaces")]
        public List<ObjectReference> FluidIntegrantScriptInterfaces { get; } = new List<ObjectReference>();
    }
}
