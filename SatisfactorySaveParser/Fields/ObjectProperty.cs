using System;
using System.Diagnostics;
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

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Str1 = reader.ReadLengthPrefixedString();
            result.Str2 = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
