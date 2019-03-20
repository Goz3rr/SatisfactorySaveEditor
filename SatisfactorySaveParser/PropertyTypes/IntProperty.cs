using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class IntProperty : SerializedProperty
    {
        public const string TypeName = nameof(IntProperty);
        public override string PropertyType => TypeName;

        public int Value { get; set; }

        public IntProperty(string propertyName, int value) : base(propertyName)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"int: {Value}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(4);
            writer.Write(0);

            writer.Write((byte)0);
            writer.Write(Value);
        }

        public static IntProperty Parse(string propertyName, BinaryReader reader)
        {
            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            return new IntProperty(propertyName, reader.ReadInt32());
        }
    }
}
