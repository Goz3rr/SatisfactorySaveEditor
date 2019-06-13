using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Class representing a reference to another object in the game
    /// </summary>
    public class ObjectReference
    {
        /// <summary>
        ///     Name of the level the referenced object is in. Empty when <see cref="PathName"/> is an absolute path
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        ///     PathName of the referenced object including the sublevel name. Acts as a unique identifier of the object
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        ///     Helper property that points to the instance of the referenced object, if the parser was able to locate it.
        ///     Setting this property will cause <see cref="LevelName"/> and <see cref="PathName"/> to be updated
        /// </summary>
        public SaveObject ReferencedObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObjectReference()
        {
        }

        public ObjectReference(string level, string path)
        {
            LevelName = level;
            PathName = path;
        }

        public override string ToString()
        {
            return $"{LevelName}::{PathName}";
        }
    }
}
