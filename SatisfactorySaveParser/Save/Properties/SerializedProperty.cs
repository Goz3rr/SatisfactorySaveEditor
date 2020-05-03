using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save.Properties
{
    public abstract class SerializedProperty
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly Dictionary<(Type, string), (PropertyInfo, Attribute)> propertyCache = new Dictionary<(Type, string), (PropertyInfo, Attribute)>();

        /// <summary>
        ///     Name of the property
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        ///     String representation of the name of the property type
        /// </summary>
        public abstract string PropertyType { get; }

        /// <summary>
        ///     Index of property when in an array
        /// </summary>
        public int Index { get; }

        public byte HasPropertyGuid { get; private set; }

        public abstract Type BackingType { get; }
        public abstract object BackingObject { get; }

        public abstract int SerializedLength { get; }

        protected SerializedProperty(string propertyName, int index)
        {
            PropertyName = propertyName;
            Index = index;
        }

        /// <summary>
        ///     Attempts to find a matching class property for this serialized property. Returns null if one can't be found.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public (PropertyInfo Property, SavePropertyAttribute Attribute) GetMatchingSaveProperty(Type targetType)
        {
            if (!propertyCache.TryGetValue((targetType, PropertyName), out (PropertyInfo Property, Attribute Attribute) found))
            {
                found = targetType.GetProperties()
                    //.Where(p => Attribute.IsDefined(p, typeof(SavePropertyAttribute)))
                    .Select(p => (Property: p, Attribute: p.GetCustomAttributes(typeof(SavePropertyAttribute), false).FirstOrDefault() as SavePropertyAttribute))
                    .SingleOrDefault(p => p.Attribute != null && p.Attribute.Name == PropertyName);

                propertyCache[(targetType, PropertyName)] = found;
            }

            return (found.Property, (SavePropertyAttribute)found.Attribute);
        }

        /// <summary>
        ///     Attempts to find a matching struct property for this serialized property. Returns null if one can't be found.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public (PropertyInfo Property, StructPropertyAttribute Attribute) GetMatchingStructProperty(Type targetType)
        {
            if (targetType == typeof(DynamicGameStruct))
                return (null, null);

            if (!propertyCache.TryGetValue((targetType, PropertyName), out (PropertyInfo Property, Attribute Attribute) found))
            {
                found = targetType.GetProperties()
                    //.Where(p => Attribute.IsDefined(p, typeof(SavePropertyAttribute)))
                    .Select(p => (Property: p, Attribute: p.GetCustomAttributes(typeof(StructPropertyAttribute), false).FirstOrDefault() as StructPropertyAttribute))
                    .SingleOrDefault(p => p.Attribute != null && p.Attribute.Name == PropertyName);

                propertyCache[(targetType, PropertyName)] = found;
            }

            return (found.Property, (StructPropertyAttribute)found.Attribute);
        }

        public virtual void AssignToProperty(IPropertyContainer saveObject, PropertyInfo info)
        {
            switch (this)
            {
                case ArrayProperty arrayProperty:
                    {
                        if (info.PropertyType.GetGenericTypeDefinition() != typeof(List<>))
                        {
                            log.Error($"Attempted to assign array {PropertyName} to non list field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        var list = info.GetValue(saveObject);
                        var addMethod = info.PropertyType.GetMethod(nameof(List<object>.Add));

                        switch (arrayProperty.Type)
                        {
                            case ByteProperty.TypeName:
                                {
                                    foreach (var obj in arrayProperty.Elements.Cast<ByteProperty>())
                                    {
                                        addMethod.Invoke(list, new[] { (object)obj.ByteValue });
                                    }
                                }
                                break;

                            case EnumProperty.TypeName:
                                {
                                    // TODO
                                    saveObject.AddDynamicProperty(this);
                                }
                                break;

                            case IntProperty.TypeName:
                                {
                                    foreach (var prop in arrayProperty.Elements.Cast<IntProperty>())
                                    {
                                        addMethod.Invoke(list, new[] { (object)prop.Value });
                                    }
                                }
                                break;

                            case ObjectProperty.TypeName:
                                {
                                    foreach (var obj in arrayProperty.Elements.Cast<ObjectProperty>())
                                    {
                                        addMethod.Invoke(list, new[] { obj.Reference });
                                    }
                                }
                                break;

                            case StructProperty.TypeName:
                                {
                                    foreach (var obj in arrayProperty.Elements.Cast<StructProperty>())
                                    {
                                        addMethod.Invoke(list, new[] { obj.Data });
                                    }
                                }
                                break;

                            case InterfaceProperty.TypeName:
                                {
                                    foreach (var obj in arrayProperty.Elements.Cast<InterfaceProperty>())
                                    {
                                        addMethod.Invoke(list, new[] { obj.Reference });
                                    }
                                }
                                break;

                            default:
                                {
                                    log.Warn($"Attempted to assign array {PropertyName} of unknown type {arrayProperty.Type}");
                                    saveObject.AddDynamicProperty(this);
                                }
                                break;
                        }
                    }
                    break;

                case StructProperty structProperty:
                    {
                        if (info.PropertyType.IsArray && structProperty.Data.GetType() == info.PropertyType.GetElementType())
                        {
                            var array = (Array)info.GetValue(saveObject);
                            array.SetValue(structProperty.Data, Index);
                            break;
                        }

                        if (structProperty.Data.GetType() != info.PropertyType)
                        {
                            log.Error($"Attempted to assign struct {PropertyName} ({structProperty.Data.GetType().Name}) to mismatched property {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        info.SetValue(saveObject, structProperty.Data);
                    }
                    break;

                case EnumProperty enumProperty:
                    {
                        if (enumProperty.Type != info.PropertyType.Name)
                        {
                            log.Error($"Attempted to assign enum {PropertyName} ({enumProperty.Type}) to mismatched property {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        // TODO: should probably already be in BackingObject
                        if (!Enum.TryParse(info.PropertyType, enumProperty.Value.Split(':').Last(), true, out object enumValue))
                        {
                            log.Error($"Failed to parse \"{enumProperty.Value}\" as {info.PropertyType.Name}");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        info.SetValue(saveObject, enumValue);
                    }
                    break;

                case ByteProperty byteProperty:
                    {
                        if (byteProperty.IsEnum)
                        {
                            if (!info.PropertyType.IsGenericType || info.PropertyType.GetGenericTypeDefinition() != typeof(EnumAsByte<>))
                            {
                                log.Error($"Attempted to assign {PropertyType} ({byteProperty.EnumType}) {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                                saveObject.AddDynamicProperty(this);
                                break;
                            }

                            var enumType = info.PropertyType.GenericTypeArguments[0];
                            if (enumType.Name != byteProperty.EnumType)
                            {
                                log.Error($"Attempted to assign {PropertyType} ({byteProperty.EnumType}) {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                                saveObject.AddDynamicProperty(this);
                                break;
                            }

                            if (!Enum.TryParse(enumType, byteProperty.EnumValue, true, out object enumValue))
                            {
                                log.Error($"Failed to parse \"{byteProperty.EnumValue}\" as {enumType.Name}");
                                saveObject.AddDynamicProperty(this);
                                break;
                            }

                            var enumAsByteType = typeof(EnumAsByte<>).MakeGenericType(new[] { enumType });
                            var instance = Activator.CreateInstance(enumAsByteType, enumValue);
                            info.SetValue(saveObject, instance);
                            break;
                        }

                        if (info.PropertyType != typeof(byte))
                        {
                            log.Error($"Attempted to assign {PropertyType} {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        info.SetValue(saveObject, byteProperty.ByteValue);
                    }
                    break;

                case MapProperty mapProperty:
                    {
                        // TODO
                        saveObject.AddDynamicProperty(this);
                    }
                    break;

                default:
                    {
                        if (info.PropertyType.IsArray && BackingType == info.PropertyType.GetElementType())
                        {
                            var array = (Array)info.GetValue(saveObject);
                            array.SetValue(BackingObject, Index);
                            break;
                        }

                        if (info.PropertyType != BackingType)
                        {
                            log.Error($"Attempted to assign {PropertyType} {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                            saveObject.AddDynamicProperty(this);
                            break;
                        }

                        info.SetValue(saveObject, BackingObject);
                    }
                    break;
            }
        }

        public abstract void Serialize(BinaryWriter writer);

        public static Type GetPropertyTypeFromName(string name)
        {
            return name switch
            {
                ArrayProperty.TypeName => typeof(ArrayProperty),
                BoolProperty.TypeName => typeof(BoolProperty),
                ByteProperty.TypeName => typeof(ByteProperty),
                EnumProperty.TypeName => typeof(EnumProperty),
                FloatProperty.TypeName => typeof(FloatProperty),
                Int64Property.TypeName => typeof(Int64Property),
                IntProperty.TypeName => typeof(IntProperty),
                MapProperty.TypeName => typeof(MapProperty),
                NameProperty.TypeName => typeof(NameProperty),
                ObjectProperty.TypeName => typeof(ObjectProperty),
                StrProperty.TypeName => typeof(StrProperty),
                StructProperty.TypeName => typeof(StructProperty),
                TextProperty.TypeName => typeof(TextProperty),
                _ => throw new NotImplementedException($"Unknown PropertyType {name}"),
            };
        }
    }
}
