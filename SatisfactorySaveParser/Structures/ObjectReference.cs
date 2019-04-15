using System;
using System.IO;

namespace SatisfactorySaveParser.Structures
{
    /// <summary>
    ///     Engine class: FObjectReferenceDisc
    /// </summary>
    public class ObjectReference : IObjectReference
    {
        public string LevelName { get; set; }
        public string PathName { get; set; }
        public SaveObject ReferencedObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObjectReference()
        {
        }

        public ObjectReference(BinaryReader reader)
        {
            LevelName = reader.ReadLengthPrefixedString();
            PathName = reader.ReadLengthPrefixedString();
        }

        public override string ToString()
        {
            return $"Level: {LevelName} Path: {PathName}";
        }
    }
}
