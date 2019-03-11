using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class BoolProperty : ISerializedField
    {
        public bool Value { get; set; }

        public override string ToString()
        {
            return $"bool: {Value}";
        }

        public static BoolProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new BoolProperty();

            var unk2 = reader.ReadInt32();
            var unk3 = reader.ReadByte();

            result.Value = reader.ReadInt32() > 0;

            var unk4 = reader.ReadByte();

            return result;
        }
    }
}
