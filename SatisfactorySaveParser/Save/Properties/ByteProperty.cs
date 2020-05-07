using System;
using System.IO;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Save.Properties.Abstractions;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ByteProperty : SerializedProperty, IBytePropertyValue
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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

        public override void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            if (IsEnum)
            {
                if (!info.PropertyType.IsGenericType || info.PropertyType.GetGenericTypeDefinition() != typeof(EnumAsByte<>))
                {
                    log.Error($"Attempted to assign {PropertyType} ({EnumType}) {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                    saveObject.AddDynamicProperty(this);
                    return;
                }

                var enumType = info.PropertyType.GenericTypeArguments[0];
                if (enumType.Name != EnumType)
                {
                    log.Error($"Attempted to assign {PropertyType} ({EnumType}) {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                    saveObject.AddDynamicProperty(this);
                    return;
                }

                if (!Enum.TryParse(enumType, EnumValue, true, out object enumValue))
                {
                    log.Error($"Failed to parse \"{EnumValue}\" as {enumType.Name}");
                    saveObject.AddDynamicProperty(this);
                    return;
                }

                var enumAsByteType = typeof(EnumAsByte<>).MakeGenericType(new[] { enumType });
                var instance = Activator.CreateInstance(enumAsByteType, enumValue);
                info.SetValue(saveObject, instance);
                return;
            }

            if (info.PropertyType != typeof(byte))
            {
                log.Error($"Attempted to assign {PropertyType} {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                saveObject.AddDynamicProperty(this);
                return;
            }

            info.SetValue(saveObject, ByteValue);
        }
    }
}
