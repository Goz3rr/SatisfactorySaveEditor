using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class FloatProperty : ISerializedField
    {
        public float Value { get; set; }

        public override string ToString()
        {
            return $"float: {Value}";
        }

        public static FloatProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new FloatProperty();

            var size = reader.ReadInt32();
            if (size != 4) throw new InvalidOperationException();

            var unk2 = reader.ReadInt32();
            var unk3 = reader.ReadByte();

            result.Value = reader.ReadSingle();

            return result;
        }
    }
}
