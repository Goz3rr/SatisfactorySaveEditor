using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using NLog;

using SatisfactorySaveParser.Game.Structs;
using SatisfactorySaveParser.Save.Properties.ArrayValues;
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

        // TODO: not currently assigned/used/implemented
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
            if (info.PropertyType.IsArray && BackingType == info.PropertyType.GetElementType())
            {
                var array = (Array)info.GetValue(saveObject);
                array.SetValue(BackingObject, Index);
                return;
            }

            if (info.PropertyType != BackingType)
            {
                log.Error($"Attempted to assign {PropertyType} {PropertyName} to incompatible backing field {info.DeclaringType}.{info.Name} ({info.PropertyType.Name})");
                saveObject.AddDynamicProperty(this);
                return;
            }

            info.SetValue(saveObject, BackingObject);
        }

        public abstract void Serialize(BinaryWriter writer);

        public static Type GetPropertyTypeFromName(string name)
        {
            // TODO: make this not nasty
            return name switch
            {
                ArrayProperty.TypeName => typeof(ArrayProperty),
                BoolProperty.TypeName => typeof(BoolProperty),
                ByteProperty.TypeName => typeof(ByteProperty),
                EnumProperty.TypeName => typeof(EnumProperty),
                FloatProperty.TypeName => typeof(FloatProperty),
                Int64Property.TypeName => typeof(Int64Property),
                Int8Property.TypeName => typeof(Int8Property),
                InterfaceProperty.TypeName => typeof(InterfaceProperty),
                IntProperty.TypeName => typeof(IntProperty),
                MapProperty.TypeName => typeof(MapProperty),
                NameProperty.TypeName => typeof(NameProperty),
                ObjectProperty.TypeName => typeof(ObjectProperty),
                SetProperty.TypeName => typeof(SetProperty),
                StrProperty.TypeName => typeof(StrProperty),
                StructProperty.TypeName => typeof(StructProperty),
                TextProperty.TypeName => typeof(TextProperty),
                _ => throw new NotImplementedException($"Unknown PropertyType {name}"),
            };
        }

        public static Type GetPropertyValueTypeFromName(string name)
        {
            return name switch
            {
                ByteProperty.TypeName => typeof(ByteArrayValue),
                EnumProperty.TypeName => typeof(EnumArrayValue),
                FloatProperty.TypeName => typeof(FloatArrayValue),
                InterfaceProperty.TypeName => typeof(InterfaceArrayValue),
                IntProperty.TypeName => typeof(IntArrayValue),
                NameProperty.TypeName => typeof(NameArrayValue),
                ObjectProperty.TypeName => typeof(ObjectArrayValue),
                StrProperty.TypeName => typeof(StrArrayValue),
                StructProperty.TypeName => typeof(StructArrayValue),
                TextProperty.TypeName => typeof(TextArrayValue),
                _ => throw new NotImplementedException($"Unknown PropertyType {name}"),
            };

        }
    }
}
