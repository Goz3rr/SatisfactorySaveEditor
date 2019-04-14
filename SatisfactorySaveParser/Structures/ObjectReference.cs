using System;
using System.IO;

namespace SatisfactorySaveParser.Structures
{
    public class ObjectReference : IObjectReference
    {
        public string Root { get; set; }
        public string Name { get; set; }
        public SaveObject ReferencedObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObjectReference()
        {
        }

        public ObjectReference(BinaryReader reader)
        {
            Root = reader.ReadLengthPrefixedString();
            Name = reader.ReadLengthPrefixedString();
        }

        public override string ToString()
        {
            return $"Root: {Root} Name: {Name}";
        }
    }
}
