using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class NameProperty : SerializedProperty
    {
        public const string TypeName = nameof(NameProperty);
        public override string PropertyType => TypeName;

        public string Value { get; set; }

        public NameProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"name: {Value}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(Value.GetSerializedLength());
            writer.Write(0);
            writer.Write((byte)0);

            writer.WriteLengthPrefixedString(Value);
        }

        public static NameProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new NameProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
