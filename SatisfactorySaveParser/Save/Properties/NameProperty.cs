using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class NameProperty : SerializedProperty
    {
        public const string TypeName = nameof(NameProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(string);
        public override object BackingObject => Value;

        public override int SerializedLength => Value.GetSerializedLength();

        public string Value { get; set; }

        public NameProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Name {PropertyName}: {Value}";
        }

        public static NameProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new NameProperty(propertyName, index);

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.WriteLengthPrefixedString(Value);
        }
    }
}
