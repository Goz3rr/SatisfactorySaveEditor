using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ByteProperty : SerializedProperty
    {
        public const string TypeName = nameof(ByteProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Type == "None" ? 1 : Value.GetSerializedLength();

        public string Type { get; set; }
        public string Value { get; set; }

        public ByteProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"byte";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            if (writeHeader)
                base.Serialize(writer, buildVersion, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.WriteLengthPrefixedString(Type);
            writer.Write((byte)0);

            if (Type == "None")
            {
                var b = byte.Parse(Value);
                writer.Write(b);
            }
            else
            {
                writer.WriteLengthPrefixedString(Value);
            }
        }

        public static ByteProperty Parse(string propertyName, int index, BinaryReader reader, out int overhead)
        {
            var result = new ByteProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            var unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            if (result.Type == "None")
            {
                result.Value = reader.ReadByte().ToString();
            }
            else
            {
                result.Value = reader.ReadLengthPrefixedString();
            }

            overhead = result.Type.Length + 6;

            return result;
        }
    }
}
