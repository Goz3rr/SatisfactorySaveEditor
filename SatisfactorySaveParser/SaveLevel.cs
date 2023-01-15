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
        public List<string> ContainedObjectInstances = new List<string>();
        
        
        /// <summary>
        /// a list of references to collected objects, that belong in this level
        /// </summary>
        public List<ObjectReference> ContainedCollectablesInstances = new List<ObjectReference>();
        
        public SaveLevel(string name)
        {
            Name = name;
        }
    }
}