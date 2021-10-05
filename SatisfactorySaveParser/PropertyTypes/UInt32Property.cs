using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class UInt32Property : SerializedProperty
    {
        public const string TypeName = nameof(UInt32Property);
        public override string PropertyType => TypeName;

        public override int SerializedLength => 4;

        public uint Value { get; set; }

        public UInt32Property(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"uint32: {Value}";
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            base.Serialize(writer, buildVersion, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.Write((byte)0);
            writer.Write(Value);
        }

        public static UInt32Property Parse(string propertyName, int index, BinaryReader reader)
        {
            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            return new UInt32Property(propertyName, index)
            {
                Value = reader.ReadUInt32()
            };
        }
    }
}
