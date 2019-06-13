namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Class representing a single saved UObject in a Satisfactory save
    /// </summary>
    public abstract class SaveObject
    {
        /// <summary>
        ///     Type of save object
        /// </summary>
        public abstract SaveObjectKind ObjectKind { get; }

        /// <summary>
        ///     Forward slash separated path of the script/prefab of this object
        ///     Can be an empty string
        /// </summary>
        public string TypePath { get; set; }

        /// <summary>
        ///     Reference to the instance of the object
        /// </summary>
        public ObjectReference Instance { get; set; }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
