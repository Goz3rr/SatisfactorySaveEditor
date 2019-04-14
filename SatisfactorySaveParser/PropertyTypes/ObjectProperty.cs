using SatisfactorySaveParser.Structures;
using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ObjectProperty : SerializedProperty, IObjectReference
    {
        public const string TypeName = nameof(ObjectProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Root.GetSerializedLength() + Name.GetSerializedLength();

        public string Root { get; set; }
        public string Name { get; set; }
        public SaveObject ReferencedObject { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ObjectProperty(string propertyName, string root = null, string name = null, int index = 0) : base(propertyName, index)
        {
            Root = root;
            Name = name;
        }

        public ObjectProperty(string propertyName, int index) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"obj: {Name}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);
            writer.Write((byte)0);

            writer.WriteLengthPrefixedString(Root);
            writer.WriteLengthPrefixedString(Name);
        }

        public static ObjectProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new ObjectProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Root = reader.ReadLengthPrefixedString();
            result.Name = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
