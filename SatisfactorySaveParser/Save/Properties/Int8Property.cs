using System;
using System.IO;

using SatisfactorySaveParser.Save.Properties.Abstractions;

namespace SatisfactorySaveParser.Save.Properties
{
    public class Int8Property : SerializedProperty, IInt8PropertyValue
    {
        public const string TypeName = nameof(Int8Property);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(byte);
        public override object BackingObject => Value;

        public override int SerializedLength => 1;

        public byte Value { get; set; }

        public Int8Property(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Int8 {PropertyName}: {Value}";
        }

        public static Int8Property Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new Int8Property(propertyName, index);

            reader.AssertNullByte();
            result.Value = reader.ReadByte();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(Value);
        }
    }
}
