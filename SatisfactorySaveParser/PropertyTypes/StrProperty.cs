using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class StrProperty : SerializedProperty
    {
        public const string TypeName = nameof(StrProperty);
        public override string PropertyType => TypeName;

        public string Value { get; set; }

        public StrProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"str: {Value}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(Value.GetSerializedLength());
            writer.Write(0);
            writer.Write((byte)0);

            writer.WriteLengthPrefixedString(Value);
        }

        public static StrProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new StrProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
