using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using NLog;

using SatisfactorySaveParser.Save.Properties;
using SatisfactorySaveParser.Save.Serialization;

namespace SatisfactorySaveParser.Save
{
    /// <summary>
    ///     Class representing a single saved UObject in a Satisfactory save
    /// </summary>
    public abstract class SaveObject : IPropertyContainer
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly List<string> missingDeserializers = new List<string>();
        private static readonly List<string> missingSerializers = new List<string>();

        private List<SerializedProperty> dynamicProperties;

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
        public ReadOnlyCollection<SerializedProperty> DynamicProperties => dynamicProperties?.AsReadOnly();

        /// <summary>
        ///     Fallback array of native bytes that are only used for certain objects when serialization logic is missing, ideally always empty
        /// </summary>
        public byte[] NativeData { get; set; }

        /// <summary>
        ///     Workaround for some objects that have 0 bytes of data, to enable an edge case while serializing
        /// </summary>
        public bool HasData { get; set; } = true;

        public override string ToString()
        {
            return TypePath;
        }

        public void AddDynamicProperty(SerializedProperty prop)
        {
            if (dynamicProperties is null)
                dynamicProperties = new List<SerializedProperty>();

            dynamicProperties.Add(prop);
        }

        /// <summary>
        ///     Default implementation to allow saves to at least load when missing logic for proper deserialization of native data.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="length"></param>
        public virtual void DeserializeNativeData(BinaryReader reader, int length)
        {
            if (!missingDeserializers.Contains(TypePath))
            {
                log.Warn($"Missing native deserializer for {ObjectKind} {TypePath} ({length} bytes)");
                missingDeserializers.Add(TypePath);
            }

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
                if (!missingSerializers.Contains(TypePath))
                {
                    log.Warn($"Missing native serializer for {ObjectKind} {TypePath} ({NativeData.Length} bytes)");
                    missingSerializers.Add(TypePath);
                }

                writer.Write(NativeData);
            }
        }
    }
}
