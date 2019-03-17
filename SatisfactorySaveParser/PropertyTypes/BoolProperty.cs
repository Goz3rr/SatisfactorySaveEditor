using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class BoolProperty : SerializedProperty
    {
        public bool Value { get; set; }

        public BoolProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"bool: {Value}";
        }

        public static BoolProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new BoolProperty(propertyName);

            result.Value = reader.ReadInt16() > 0;

            return result;
        }
    }
}
