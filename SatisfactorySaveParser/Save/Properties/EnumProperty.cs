using System;
using System.IO;
using System.Linq;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Save.Properties.Abstractions;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class EnumProperty : SerializedProperty, IEnumPropertyValue
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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

            result.ReadPropertyGuid(reader);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(Type);
            WritePropertyGuid(writer);
            writer.WriteLengthPrefixedString(Value);
        }

        public override void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            if (Type != info.PropertyType.Name)
            {
                log.Error($"Attempted to assign enum {PropertyName} ({Type}) to mismatched property {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                saveObject.AddDynamicProperty(this);
                return;
            }

            // TODO: should probably already be in BackingObject
            if (!Enum.TryParse(info.PropertyType, Value.Split(':').Last(), true, out object enumValue))
            {
                log.Error($"Failed to parse \"{Value}\" as {info.PropertyType.Name}");
                saveObject.AddDynamicProperty(this);
                return;
            }

            info.SetValue(saveObject, enumValue);
        }
    }
}
