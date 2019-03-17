using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class IntProperty : SerializedProperty
    {
        public int Value { get; set; }

        public IntProperty(string propertyName, int value) : base(propertyName)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"int: {Value}";
        }

        public static IntProperty Parse(string propertyName, BinaryReader reader)
        {
            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            return new IntProperty(propertyName, reader.ReadInt32());
        }
    }
}
