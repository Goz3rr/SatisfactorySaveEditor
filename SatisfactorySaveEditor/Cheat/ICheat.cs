using SatisfactorySaveParser.Save;
using System.Collections.Generic;

namespace SatisfactorySaveEditor.Cheat
{
    /// <summary>
    /// Defines a single cheat and a check whether this cheat can be executed. Additional info about the cheat can be specified with a <see cref="CheatInfoAttribute"/>.
    /// </summary>
    public interface ICheat
    {
        /// <summary>
        /// Checks whether this cheat can be executed
        /// <para>Returning false will only stop the editor from executing the cheat, any sort of error MessageBoxes is up to the implementation</para>
        /// </summary>
        /// <param name="session">Loaded save</param>
        /// <param name="rightPanelSelected">Item selected in the right panel</param>
        /// <param name="multiSelected">Items with checkboxes in the left tree</param>
        /// <returns>Whether this cheat can be executed</returns>
        bool CanExecute(FGSaveSession session, SaveObject rightPanelSelected, List<SaveObject> multiSelected);

        /// <summary>
        /// Executes this cheat
        /// </summary>
        /// <param name="session">Loaded save</param>
        /// <param name="rightPanelSelected">Item selected in the right panel</param>
        /// <param name="multiSelected">Items with checkboxes in the left tree</param>
        void Execute(FGSaveSession session, SaveObject rightPanelSelected, List<SaveObject> multiSelected);
    }
}
