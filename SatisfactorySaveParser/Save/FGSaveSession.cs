using System.Collections.Generic;

namespace SatisfactorySaveParser.Save
{
    public class FGSaveSession
    {
        /// <summary>
        ///     Save header which contains information like the version and map info
        /// </summary>
        public FSaveHeader Header { get; set; }

        /// <summary>
        ///     Objects contained within this save
        /// </summary>
        public List<SaveObject> Objects { get; } = new List<SaveObject>();

        /// <summary>
        ///     List of object references of all destroyed/collected objects in the world (nut/berry bushes, slugs, etc)
        /// </summary>
        public List<ObjectReference> DestroyedActors { get; } = new List<ObjectReference>();
    }
}
