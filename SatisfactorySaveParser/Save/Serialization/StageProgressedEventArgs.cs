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
        public int Current { get; set; }

        /// <summary>
        ///     The target amount of progress to be made before the current stage is done
        /// </summary>
        public int Total { get; set; }
    }
}
