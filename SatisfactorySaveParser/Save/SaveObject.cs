using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using SatisfactorySaveParser.Save.Properties;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Class representing a single saved UObject in a Satisfactory save
    /// </summary>
    public abstract class SaveObject
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Type of save object
        /// </summary>
        public abstract SaveObjectKind ObjectKind { get; }

        /// <summary>
        ///     Forward slash separated path of the script/prefab of this object
        ///     Can be an empty string
        /// </summary>
        public string TypePath { get; set; }

        /// <summary>
        ///     Reference to the instance of the object
        /// </summary>
        public ObjectReference Instance { get; set; }

        /// <summary>
        ///     Fallback list of properties that had no matching class property
        /// </summary>
        public List<SerializedProperty> DynamicProperties { get; } = new List<SerializedProperty>();

        /// <summary>
        ///     Fallback array of native bytes that are only used for certain objects when serialization logic is missing, ideally always empty
        /// </summary>
        public byte[] NativeData { get; set; } = null;

        public override string ToString()
        {
            return TypePath;
        }

        /// <summary>
        ///     Default implementation to allow saves to at least load when missing logic for proper deserialization of native data.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        public virtual void DeserializeNativeData(BinaryReader reader, int length)
        {
            log.Warn($"Missing native deserializer for {ObjectKind} {TypePath}");
            NativeData = reader.ReadBytes(length);
        }

        /// <summary>
        ///     Default implementation to allow saves to at least save when missing logic for proper serialization of native data.
        /// </summary>
        /// <param name="reader"></param>
        public virtual void SerializeNativeData(BinaryWriter writer)
        {
            if (NativeData != null)
            {
                log.Warn($"Missing native serializer for {ObjectKind} {TypePath}");
                writer.Write(NativeData);
            }
        }

        /// <summary>
        ///     Attempts to find a matching class property for the serialized property. Returns null if one can't be found.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public (PropertyInfo Property, SavePropertyAttribute Attribute) GetMatchingProperty(SerializedProperty prop)
        {
            var found = GetType().GetProperties()
                //.Where(p => Attribute.IsDefined(p, typeof(SavePropertyAttribute)))
                .Select(p => (Property: p, Attribute: p.GetCustomAttributes(typeof(SavePropertyAttribute), false).SingleOrDefault() as SavePropertyAttribute))
                .SingleOrDefault(p => p.Attribute != null && p.Attribute.Name == prop.PropertyName);

            return found;
        }
    }
}
