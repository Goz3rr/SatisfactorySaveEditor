using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ObjectProperty : SerializedProperty, IObjectPropertyValue
    {
        public const string TypeName = nameof(ObjectProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(ObjectReference);
        public override object BackingObject => Reference;

        public override int SerializedLength => Reference.SerializedLength;

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

            result.ReadPropertyGuid(reader);
            result.Reference = reader.ReadObjectReference();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            WritePropertyGuid(writer);
            writer.Write(Reference);
        }
    }
}
