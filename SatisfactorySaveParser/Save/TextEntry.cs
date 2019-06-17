namespace SatisfactorySaveParser.Save
{
    public class TextEntry
    {
        /// <summary>
        ///     Unknown integer
        /// </summary>
        public int UnknownInt { get; set; }

        /// <summary>
        ///     Unknown string value, possibly relating to string table
        /// </summary>
        public string UnknownString { get; set; }

        /// <summary>
        ///     Main text value
        /// </summary>
        public string Value { get; set; }
    }
}
