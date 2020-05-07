using System.Collections.Generic;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser.Game.Structs
{
    /// <summary>
    ///     Contains data about the research conducted
    /// </summary>
    [GameStruct("ResearchData")]
    public class FResearchData : GameStruct
    {
        public override string StructName => "ResearchData";

        /// <summary>
        ///     The schematic that holds the research data
        /// </summary>
        [StructProperty("Schematic")]
        public ObjectReference Schematic { get; set; }

        /// <summary>
        ///     The research tree that initiated the research
        /// </summary>
        [StructProperty("InitiatingResearchTree")]
        public ObjectReference InitiatingResearchTree { get; set; }

        /// <summary>
        ///     The rewards that have been generated for this schematic. 
        ///     This is used for example to store randomized alternate recipe schematics when analyzing a hard drive 
        ///     This array can be empty since most schematics use the unlock system except hard drives that generate rewards when research is initialized
        /// </summary>
        [StructProperty("PendingRewards")]
        public List<ObjectReference> PendingRewards { get; } = new List<ObjectReference>();
    }
}
