namespace SatisfactorySaveParser.Game.Structs
{
    [GameStruct("ResearchTime")]
    public class FResearchTime : GameStruct
    {
        public override string StructName => "ResearchTime";

        /// <summary>
        ///     The research entry that contains data about the research conducted
        /// </summary>
        [StructProperty("ResearchData")]
        public FResearchData ResearchRecipe { get; set; }

        /// <summary>
        ///     The time stamp for when the research is completed. When saved it represents how much time is left for research
        /// </summary>
        [StructProperty("ResearchCompleteTimestamp")]
        public float ResearchCompleteTimestamp { get; set; }
    }
}
