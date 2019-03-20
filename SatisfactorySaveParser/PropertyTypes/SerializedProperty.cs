using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public abstract class SerializedProperty
    {
        public string PropertyName { get; }
        public abstract string PropertyType { get; }

        public SerializedProperty(string propertyName)
        {
            PropertyName = propertyName;
        }

        public virtual void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            writer.WriteLengthPrefixedString(PropertyName);
            writer.WriteLengthPrefixedString(PropertyType);
        }
    }
}