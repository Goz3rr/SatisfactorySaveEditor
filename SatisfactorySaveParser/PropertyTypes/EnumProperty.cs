using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class EnumProperty : SerializedProperty
    {
        public const string TypeName = nameof(EnumProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Name.GetSerializedLength();

        public string Name { get; set; }
        public string Type { get; set; }

        public EnumProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }
        public override string ToString()
        {
            return $"enum: {Name}";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            base.Serialize(writer, buildVersion, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.WriteLengthPrefixedString(Type);
            writer.Write((byte)0);
            writer.WriteLengthPrefixedString(Name);
        }

        public static EnumProperty Parse(string propertyName, int index, BinaryReader reader, out int overhead)
        {
            var result = new EnumProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            overhead = result.Type.Length + 6;

            var unk4 = reader.ReadByte();
            Trace.Assert(unk4 == 0);

            result.Name = reader.ReadLengthPrefixedString();
            //result.Value = reader.ReadInt32();

            return result;
        }
    }
}
