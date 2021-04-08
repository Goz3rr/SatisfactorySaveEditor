using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.Abstractions;
using SatisfactorySaveParser.Save.Properties.ArrayValues;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ArrayProperty : SerializedProperty
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public const string TypeName = nameof(ArrayProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(List<IArrayElement>);
        public override object BackingObject => Elements;

        public override int SerializedLength => 0;

        /// <summary>
        ///     String representation of the Property type this array consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the array
        /// </summary>
        public List<IArrayElement> Elements { get; } = new List<IArrayElement>();

        public ArrayProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static ArrayProperty Parse(BinaryReader reader, string propertyName, int index, int buildVersion, out int overhead)
        {
            var result = new ArrayProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            result.ReadPropertyGuid(reader);

            overhead = result.Type.GetSerializedLength() + 1;

            var count = reader.ReadInt32();

            switch (result.Type)
            {
                case StructProperty.TypeName:
                    {
                        var name = reader.ReadLengthPrefixedString();

                        var propertyType = reader.ReadLengthPrefixedString();
                        Trace.Assert(propertyType == "StructProperty");

                        var structSize = reader.ReadInt32();
                        var structIndex = reader.ReadInt32();

                        var structType = reader.ReadLengthPrefixedString();

                        var unk1 = reader.ReadInt32();
                        var unk2 = reader.ReadInt32();
                        var unk3 = reader.ReadInt32();
                        var unk4 = reader.ReadInt32();
                        var unk5 = reader.ReadByte();

                        for (var i = 0; i < count; i++)
                        {
                            var structObj = GameStructFactory.CreateFromType(structType);
                            structObj.Deserialize(reader, buildVersion);

                            result.Elements.Add(new StructArrayValue()
                            {
                                Data = structObj
                            });
                        }
                    }
                    break;

                default:
                    {
                        for (var i = 0; i < count; i++)
                        {
                            result.Elements.Add(SatisfactorySaveSerializer.DeserializeArrayElement(result.Type, reader, buildVersion));
                        }
                    }
                    break;
            }

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }

        public override void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            if (info.PropertyType.GetGenericTypeDefinition() != typeof(List<>))
            {
                log.Error($"Attempted to assign array {PropertyName} to non list field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                saveObject.AddDynamicProperty(this);
                return;
            }

            var propertyType = info.PropertyType.GetGenericArguments()[0];
            var mismatchedType = Elements.FirstOrDefault(e => !e.BackingType.IsAssignableFrom(propertyType));
            if (mismatchedType != null)
            {
                log.Error($"Attempted to insert {mismatchedType.BackingType} into {info.DeclaringType}.{info.Name} of {propertyType}");
                saveObject.AddDynamicProperty(this);
                return;
            }

            var list = info.GetValue(saveObject);
            var addMethod = info.PropertyType.GetMethod(nameof(List<object>.Add));

            switch (Type)
            {
                case ByteProperty.TypeName:
                    {
                        foreach (var obj in Elements.Cast<IBytePropertyValue>())
                        {
                            addMethod.Invoke(list, new[] { (object)obj.ByteValue });
                        }
                    }
                    break;

                case EnumProperty.TypeName:
                    {
                        // TODO: add assigning of enums
                        saveObject.AddDynamicProperty(this);
                    }
                    break;

                case IntProperty.TypeName:
                    {
                        foreach (var prop in Elements.Cast<IIntPropertyValue>())
                        {
                            addMethod.Invoke(list, new[] { (object)prop.Value });
                        }
                    }
                    break;

                case ObjectProperty.TypeName:
                    {
                        foreach (var obj in Elements.Cast<IObjectPropertyValue>())
                        {
                            addMethod.Invoke(list, new[] { obj.Reference });
                        }
                    }
                    break;

                case StructProperty.TypeName:
                    {
                        foreach (var obj in Elements.Cast<IStructPropertyValue>())
                        {
                            addMethod.Invoke(list, new[] { obj.Data });
                        }
                    }
                    break;

                case InterfaceProperty.TypeName:
                    {
                        foreach (var obj in Elements.Cast<IInterfacePropertyValue>())
                        {
                            addMethod.Invoke(list, new[] { obj.Reference });
                        }
                    }
                    break;

                default:
                    {
                        log.Warn($"Attempted to assign array {PropertyName} of unknown type {Type}");
                        saveObject.AddDynamicProperty(this);
                    }
                    break;
            }
        }
    }
}
