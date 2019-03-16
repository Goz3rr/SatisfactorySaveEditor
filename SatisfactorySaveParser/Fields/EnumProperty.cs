using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class EnumProperty : ISerializedField
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"enum: {Name}";
        }

        public static EnumProperty Parse(string fieldName, BinaryReader reader, out int overhead)
        {
            var result = new EnumProperty();

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
