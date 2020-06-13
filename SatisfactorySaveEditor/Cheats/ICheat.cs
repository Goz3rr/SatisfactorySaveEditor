using SatisfactorySaveEditor.Model;

using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Cheats
{
    public interface ICheat
    {
        string Name { get; }

        /// <summary>
        ///     Activate the cheat
        /// </summary>
        /// <param name="rootItem">SaveObjectModel to apply the cheat on</param>
        /// <param name="saveGame"></param>
        /// <returns>true if succesfull and the SaveObjectModel was mutated, false on failure</returns>
        bool Apply(SaveObjectModel rootItem, SatisfactorySave saveGame);
    }
}
