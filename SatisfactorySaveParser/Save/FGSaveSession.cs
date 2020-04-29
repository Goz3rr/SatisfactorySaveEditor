using System;
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

        /// <summary>
        ///     Tries to find an object with the specified type, instantiates it, adds it to the <see cref="Objects"/> list and returns the instance.
        /// </summary>
        /// <param name="type">The full type path of the object to add</param>
        /// <returns></returns>
        public SaveObject AddObject(string type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Instantiates an object of the specified type, adds it to the <see cref="Objects"/> list and returns the instance
        /// </summary>
        /// <typeparam name="T">The SaveObject to add</typeparam>
        /// <returns></returns>
        public T AddObject<T>() where T : SaveObject
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Finds the specified object and tries to remove all traces of it.
        ///     This includes child components which are also deleted and any <see cref="ObjectReference"/> pointing to this object is cleared.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns>True if object was found and removed, false if not found</returns>
        public bool RemoveObject(string instance)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Finds the specified object and tries to remove all traces of it.
        ///     This includes child components which are also deleted and any <see cref="ObjectReference"/> pointing to this object is cleared.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if object was found and removed, false if not found</returns>
        public bool RemoveObject(SaveObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
