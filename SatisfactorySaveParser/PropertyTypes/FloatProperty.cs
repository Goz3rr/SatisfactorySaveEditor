using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class FloatProperty : SerializedProperty
    {
        public const string TypeName = nameof(FloatProperty);
        public override string PropertyType => TypeName;

        public float Value { get; set; }

        public FloatProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"float: {Value}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(4);
            writer.Write(0);

            writer.Write((byte)0);
            writer.Write(Value);
        }

        public static FloatProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new FloatProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadSingle();

            return result;
        }
    }
}
