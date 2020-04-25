using System;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Class representing a reference to another object in the game
    /// </summary>
    public class ObjectReference : IEquatable<ObjectReference>
    {
        /// <summary>
        ///     Name of the level the referenced object is in. Empty when <see cref="PathName"/> is an absolute path
        /// </summary>
        public string LevelName { get; }
        /// <summary>
        ///     PathName of the referenced object including the sublevel name. Acts as a unique identifier of the object
        /// </summary>
        public string PathName { get; }

        /// <summary>
        ///     Helper property that points to the instance of the referenced object, if the parser was able to locate it.
        ///     Setting this property will cause <see cref="LevelName"/> and <see cref="PathName"/> to be updated
        /// </summary>
        public SaveObject ReferencedObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsInLevel => !String.IsNullOrEmpty(LevelName);

        /// <summary>
        ///     Get the serialized length of this ObjectReference in bytes
        /// </summary>
        public int SerializedLength => LevelName.GetSerializedLength() + PathName.GetSerializedLength();

        public ObjectReference(string level, string path)
        {
            LevelName = level;
            PathName = path;
        }

        public override string ToString()
        {
            return $"{LevelName}::{PathName}";
        }

        public bool Equals(ObjectReference other)
        {
            if (other == null) return false;

            return LevelName == other.LevelName &&
                PathName == other.PathName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ObjectReference);
        }

        public override int GetHashCode()
        {
            return LevelName.GetHashCode(StringComparison.InvariantCulture) + 17 * PathName.GetHashCode(StringComparison.InvariantCulture);
        }
    }
}
