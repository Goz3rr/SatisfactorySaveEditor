using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class IntProperty : ISerializedField
    {
        public int Value { get; set; }

        public IntProperty(int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"int: {Value}";
        }

        public static IntProperty Parse(string fieldName, BinaryReader reader)
        {
            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            return new IntProperty(reader.ReadInt32());
        }
    }
}
