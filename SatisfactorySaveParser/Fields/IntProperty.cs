using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class IntProperty : ISerializedField
    {
        public int Value { get; set; }

        public override string ToString()
        {
            return $"int: {Value}";
        }

        public static IntProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new IntProperty();

            var size = reader.ReadInt32();
            if (size != 4) throw new InvalidOperationException();

            var unk2 = reader.ReadInt32();
            var unk3 = reader.ReadByte();

            result.Value = reader.ReadInt32();

            return result;
        }
    }
}
