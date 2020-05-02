using System;

namespace SatisfactorySaveParser.Save.Serialization
{
    public class StageProgressedEventArgs : EventArgs
    {
        /// <summary>
        ///     Percentage of the current stage progress
        /// </summary>
        public float Progress { get; set; }

        /// <summary>
        ///     How much of the current stage has been completed
        /// </summary>
        public long Current { get; set; }

        /// <summary>
        ///     The target amount of progress to be made before the current stage is done.
        ///     Can be -1 in case the current stage does not have incrementing progress and should instead be considered to take an indetermined amount of time
        /// </summary>
        public long Total { get; set; }
    }
}
