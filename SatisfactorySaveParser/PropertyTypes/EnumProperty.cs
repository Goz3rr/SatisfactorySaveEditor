using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class EnumProperty : SerializedProperty
    {
        public const string TypeName = nameof(EnumProperty);
        public override string PropertyType => TypeName;

        public int Value { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public EnumProperty(string propertyName) : base(propertyName)
        {
        }
        public override string ToString()
        {
            return $"enum: {Name}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(Name.GetSerializedLength());
            writer.Write(0);

            writer.WriteLengthPrefixedString(Type);
            writer.Write((byte)0);
            writer.WriteLengthPrefixedString(Name);
        }

        public static EnumProperty Parse(string propertyName, BinaryReader reader, out int overhead)
        {
            var result = new EnumProperty(propertyName);

            result.Type = reader.ReadLengthPrefixedString();
            overhead = result.Type.Length + 6;

            var unk4 = reader.ReadByte();
            Trace.Assert(unk4 == 0);

            result.Name = reader.ReadLengthPrefixedString();
            //result.Value = reader.ReadInt32();

            return result;
        }
    }
}
