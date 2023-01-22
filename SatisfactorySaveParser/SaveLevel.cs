using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SatisfactorySaveParser.Structures;

namespace SatisfactorySaveParser
{
    public class SaveLevel
    {
        /// <summary>
        /// The name of the level
        /// </summary>
        public string Name;

        /// <summary>
        /// a list of object instance names that belong in this level.
        /// </summary>
        public List<SaveObject> Entries = new List<SaveObject>();
        
        
        /// <summary>
        /// a list of references to collected objects, that belong in this level
        /// </summary>
        public List<ObjectReference> CollectedObjects = new List<ObjectReference>();
        
        public SaveLevel(string name)
        {
            Name = name;
        }
    }
}