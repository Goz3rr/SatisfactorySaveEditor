using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ObjectProperty : SerializedProperty
    {
        public const string TypeName = nameof(ObjectProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(ObjectReference);
        public override object BackingObject => Reference;

        public override int SerializedLength => Reference.GetSerializedLength();

        public ObjectReference Reference { get; set; }

        public ObjectProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Object {PropertyName}: {Reference}";
        }

        public static ObjectProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new ObjectProperty(propertyName, index);

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            result.Reference = reader.ReadObjectReference();
            return result;
        }
    }
}
