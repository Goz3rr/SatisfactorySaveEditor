using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties
{
    public class FloatProperty : SerializedProperty, IFloatPropertyValue
    {
        public const string TypeName = nameof(FloatProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(float);
        public override object BackingObject => Value;

        public override int SerializedLength => 4;

        public float Value { get; set; }

        public FloatProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Float {PropertyName}: {Value}";
        }

        public static FloatProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new FloatProperty(propertyName, index);

            reader.AssertNullByte();

            result.Value = reader.ReadSingle();
            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(Value);
        }
    }
}
