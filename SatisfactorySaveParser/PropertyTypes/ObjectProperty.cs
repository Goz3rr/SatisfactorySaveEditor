using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ObjectProperty : SerializedProperty
    {
        public const string TypeName = nameof(ObjectProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Str1.GetSerializedLength() + Str2.GetSerializedLength();

        public string Str1 { get; set; }
        public string Str2 { get; set; }

        public ObjectProperty(string propertyName, string str1 = null, string str2 = null, int index = 0) : base(propertyName, index)
        {
            Str1 = str1;
            Str2 = str2;
        }

        public ObjectProperty(string propertyName, int index) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"obj: {Str2}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);
            writer.Write((byte)0);

            writer.WriteLengthPrefixedString(Str1);
            writer.WriteLengthPrefixedString(Str2);
        }

        public static ObjectProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new ObjectProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Str1 = reader.ReadLengthPrefixedString();
            result.Str2 = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
