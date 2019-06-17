using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ByteProperty : SerializedProperty
    {
        public const string TypeName = nameof(ByteProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(object);
        public override object BackingObject => null;

        public override int SerializedLength => EnumType == "None" ? 1 : EnumValue.GetSerializedLength();

        /// <summary>
        ///     String used to store the enum type when an enum is forced to be saved as a byte (??). "None" if not an enum
        /// </summary>
        public string EnumType { get; set; }

        /// <summary>
        ///     String representation of the enum value. Only valid when <see cref="EnumType"/> is not "None"
        /// </summary>
        public string EnumValue { get; set; }

        /// <summary>
        ///     Byte value. Only valid when <see cref="EnumType"/> is "None"
        /// </summary>
        public byte ByteValue { get; set; }


        public ByteProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            if (EnumType == "None")
                return $"Byte {PropertyName}: {ByteValue}";

            return $"Byte {PropertyName}: {EnumType}::{EnumValue}";
        }

        public static ByteProperty Deserialize(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new ByteProperty(propertyName, index)
            {
                EnumType = reader.ReadLengthPrefixedString()
            };

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            if (result.EnumType == "None")
            {
                result.ByteValue = reader.ReadByte();
            }
            else
            {
                result.EnumValue = reader.ReadLengthPrefixedString();
            }

            overhead = result.EnumType.GetSerializedLength() + 1;

            return result;
        }
    }
}
