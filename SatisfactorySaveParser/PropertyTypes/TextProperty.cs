using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class TextProperty : SerializedProperty
    {
        public const string TypeName = nameof(TextProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => 13 + Value.GetSerializedLength();

        public int Unknown4 { get; set; }
        public string Value { get; set; }

        public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"text";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);
            writer.Write((byte)0);

            writer.Write(Unknown4);

            writer.Write(0);
            writer.Write(0);
            writer.Write((byte)0);

            writer.WriteLengthPrefixedString(Value);
        }

        public static TextProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new TextProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Unknown4 = reader.ReadInt32();

            var unk5 = reader.ReadInt32();
            Trace.Assert(unk5 == 0);

            var unk6 = reader.ReadInt32();
            Trace.Assert(unk6 == 0);

            var unk7 = reader.ReadByte();
            Trace.Assert(unk7 == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
