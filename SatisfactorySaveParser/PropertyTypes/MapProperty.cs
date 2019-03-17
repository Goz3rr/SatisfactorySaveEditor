using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class MapProperty : SerializedProperty
    {
        public const string TypeName = nameof(MapProperty);

        public MapProperty(string propertyName) : base(propertyName)
        {
        }

        public static MapProperty Parse(string propertyName, BinaryReader reader, int size, out int overhead)
        {
            var keyType = reader.ReadLengthPrefixedString();
            var valueType = reader.ReadLengthPrefixedString();
            overhead = keyType.Length + valueType.Length + 11;


            var bytes = reader.ReadBytes(size + 1);

            return new MapProperty(propertyName);
        }
    }
}
