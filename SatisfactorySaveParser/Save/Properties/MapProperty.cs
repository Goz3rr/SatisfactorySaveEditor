using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.ArrayValues;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class MapProperty : SerializedProperty
    {
        public const string TypeName = nameof(MapProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(Dictionary<,>);
        public override object BackingObject => Elements;

        public override int SerializedLength => 0;

        public string KeyType { get; set; }
        public string ValueType { get; set; }
        public IDictionary Elements { get; private set; }

        public MapProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Map<{KeyType},{ValueType}> {PropertyName}";
        }

        public static MapProperty Deserialize(BinaryReader reader, string propertyName, int index, int buildVersion, out int overhead)
        {
            var result = new MapProperty(propertyName, index)
            {
                KeyType = reader.ReadLengthPrefixedString(),
                ValueType = reader.ReadLengthPrefixedString()
            };

            overhead = result.KeyType.GetSerializedLength() + result.ValueType.GetSerializedLength() + 1;

            var keyType = GetPropertyValueTypeFromName(result.KeyType);
            var valueType = GetPropertyValueTypeFromName(result.ValueType);
            result.Elements = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

            result.ReadPropertyGuid(reader);
            reader.AssertNullInt32();

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var key = SatisfactorySaveSerializer.DeserializeArrayElement(result.KeyType, reader, buildVersion);

                IArrayElement value;
                switch (result.ValueType)
                {
                    case StructProperty.TypeName:
                        {
                            var gameStruct = new DynamicGameStruct(null);
                            gameStruct.Deserialize(reader, buildVersion);
                            value = new StructArrayValue()
                            {
                                Data = gameStruct
                            };
                        }
                        break;
                    default:
                        value = SatisfactorySaveSerializer.DeserializeArrayElement(result.ValueType, reader, buildVersion);
                        break;
                }

                result.Elements[key] = value;
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            // TODO: add assigning of maps
            saveObject.AddDynamicProperty(this);
        }
    }
}
