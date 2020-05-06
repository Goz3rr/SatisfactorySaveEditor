using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using SatisfactorySaveParser.Game.Structs;

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

        public static MapProperty Deserialize(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new MapProperty(propertyName, index)
            {
                KeyType = reader.ReadLengthPrefixedString(),
                ValueType = reader.ReadLengthPrefixedString()
            };

            overhead = result.KeyType.GetSerializedLength() + result.ValueType.GetSerializedLength() + 1;

            var keyType = GetPropertyTypeFromName(result.KeyType);
            var valueType = GetPropertyTypeFromName(result.ValueType);
            result.Elements = (IDictionary)Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(keyType, valueType));

            reader.AssertNullByte();
            reader.AssertNullInt32();

            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                object key, value;

                switch (result.KeyType)
                {
                    case IntProperty.TypeName:
                        {
                            key = new IntProperty(null)
                            {
                                Value = reader.ReadInt32()
                            };
                        }
                        break;
                    case ObjectProperty.TypeName:
                        {
                            key = new ObjectProperty(null)
                            {
                                Reference = reader.ReadObjectReference()
                            };
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unimplemented Map KeyType: {result.KeyType}");
                }

                switch (result.ValueType)
                {
                    case ByteProperty.TypeName:
                        {
                            value = new ByteProperty(null)
                            {
                                ByteValue = reader.ReadByte()
                            };
                        }
                        break;
                    case StructProperty.TypeName:
                        {
                            var gameStruct = new DynamicGameStruct(null);
                            gameStruct.Deserialize(reader);
                            value = new StructProperty(null)
                            {
                                Data = gameStruct
                            };
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Unimplemented Map ValueType: {result.ValueType}");
                }

                result.Elements[key] = value;
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
