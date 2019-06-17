using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class StrProperty : SerializedProperty
    {
        public const string TypeName = nameof(StrProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(string);
        public override object BackingObject => Value;

        public override int SerializedLength => Value.GetSerializedLength();

        public string Value { get; set; }

        public StrProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"String {PropertyName}: {Value}";
        }

        public static StrProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new StrProperty(propertyName, index);

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
