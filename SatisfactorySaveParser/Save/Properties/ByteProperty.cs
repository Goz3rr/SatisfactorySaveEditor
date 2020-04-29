using System;
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

        /// <summary>
        ///     Indicates if this ByteProperty is holding a <see cref="EnumAsByte{T}"/>
        /// </summary>
        public bool IsEnum => EnumType != null && EnumType != "None";

        public ByteProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            if (IsEnum)
                return $"Byte {PropertyName}: {EnumType}::{EnumValue}";

            return $"Byte {PropertyName}: {ByteValue}";
        }

        public static ByteProperty Deserialize(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new ByteProperty(propertyName, index)
            {
                EnumType = reader.ReadLengthPrefixedString()
            };

            reader.AssertNullByte();

            if (result.IsEnum)
            {
                result.EnumValue = reader.ReadLengthPrefixedString();
            }
            else
            {
                result.ByteValue = reader.ReadByte();
            }

            overhead = result.EnumType.GetSerializedLength() + 1;

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(EnumType);
            writer.Write((byte)0);

            if (IsEnum)
            {
                writer.WriteLengthPrefixedString(EnumValue);
            }
            else
            {
                writer.Write(ByteValue);
            }
        }
    }
}
