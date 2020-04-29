using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class EnumProperty : SerializedProperty
    {
        public const string TypeName = nameof(EnumProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(Enum);
        public override object BackingObject => Value;

        public override int SerializedLength => Value.GetSerializedLength();

        public string Type { get; set; }
        public string Value { get; set; }

        public EnumProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Enum {PropertyName}: {Value}";
        }

        public static EnumProperty Deserialize(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new EnumProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.GetSerializedLength() + 1;

            reader.AssertNullByte();

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(Type);
            writer.Write((byte)0);
            writer.WriteLengthPrefixedString(Value);
        }
    }
}
