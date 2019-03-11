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

            var unk3 = reader.ReadByte();

            result.Value = reader.ReadInt32();

            return result;
        }
    }
}
