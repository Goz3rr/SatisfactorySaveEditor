using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class BoolProperty : SerializedProperty
    {
        public const string TypeName = nameof(BoolProperty);
        public override string PropertyType => TypeName;

        public bool Value { get; set; }

        public BoolProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"bool: {Value}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(0);
            writer.Write(0);

            writer.Write((short)(Value ? 1 : 0));
        }

        public static BoolProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new BoolProperty(propertyName);

            result.Value = reader.ReadInt16() > 0;

            return result;
        }
    }
}
