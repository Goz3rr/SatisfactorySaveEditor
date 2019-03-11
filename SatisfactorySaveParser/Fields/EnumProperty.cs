using System;
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

        public static EnumProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new EnumProperty();

            var size = reader.ReadInt32();
            //if (size != 4) throw new InvalidOperationException();

            var unk2 = reader.ReadInt32();

            result.Type = reader.ReadLengthPrefixedString();
            var unk4 = reader.ReadByte();
            result.Name = reader.ReadLengthPrefixedString();
            //result.Value = reader.ReadInt32();

            return result;
        }
    }
}
