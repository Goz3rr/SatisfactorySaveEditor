using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class FloatProperty : SerializedProperty
    {
        public const string TypeName = nameof(FloatProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => 4;
        public float Value { get; set; }

        public FloatProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"float: {Value}";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            base.Serialize(writer, buildVersion, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.Write((byte)0);
            writer.Write(Value);
        }

        public static FloatProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new FloatProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadSingle();

            return result;
        }
    }
}
