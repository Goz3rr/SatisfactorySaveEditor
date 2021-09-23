using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class DoubleProperty : SerializedProperty
    {
        public const string TypeName = nameof(DoubleProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => 4;
        public double Value { get; set; }

        public DoubleProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"double: {Value}";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            base.Serialize(writer, buildVersion, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.Write((byte)0);
            writer.Write(Value);
        }

        public static DoubleProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new DoubleProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadDouble();

            return result;
        }
    }
}
