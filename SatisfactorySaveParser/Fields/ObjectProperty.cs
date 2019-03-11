using System;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class ObjectProperty : ISerializedField
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }

        public override string ToString()
        {
            return $"obj: {Str2}";
        }

        public static ObjectProperty Parse(string fieldName, BinaryReader reader)
        {
            var result = new ObjectProperty();

            var size = reader.ReadInt32();
            //if (size != 4) throw new InvalidOperationException();

            var unk2 = reader.ReadInt32();
            var unk3 = reader.ReadByte();

            var before = reader.BaseStream.Position;
            result.Str1 = reader.ReadLengthPrefixedString();
            result.Str2 = reader.ReadLengthPrefixedString();
            var after = reader.BaseStream.Position;

            if (before + size != after)
            {
                throw new InvalidOperationException($"Expected {size} bytes read but got {after - before}");
            }

            return result;
        }
    }
}
