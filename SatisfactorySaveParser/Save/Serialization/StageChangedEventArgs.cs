using System;

namespace SatisfactorySaveParser.Save.Serialization
{
    public class StageChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Current stage that is being done
        /// </summary>
        public SerializerStage Stage { get; set; }

        /// <summary>
        ///     The current stage, increments with each new stage
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        ///     The total amount of stages that have to be run through
        /// </summary>
        public int Total { get; set; }
    }
}
