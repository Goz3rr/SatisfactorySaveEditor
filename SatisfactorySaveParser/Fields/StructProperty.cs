using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Fields
{
    public class StructProperty : ISerializedField
    {

        public string Type { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public byte Unk5 { get; set; }

        public byte[] Data { get; set; }

        public override string ToString()
        {
            return $"";
        }

        public static StructProperty Parse(string fieldName, BinaryReader reader, int size, out int overhead)
        {
            var result = new StructProperty();

            result.Type = reader.ReadLengthPrefixedString();
            overhead = result.Type.Length + 22;

            result.Unk1 = reader.ReadInt32();
            Trace.Assert(result.Unk1 == 0);

            result.Unk2 = reader.ReadInt32();
            Trace.Assert(result.Unk2 == 0);

            result.Unk3 = reader.ReadInt32();
            Trace.Assert(result.Unk3 == 0);

            result.Unk4 = reader.ReadInt32();
            Trace.Assert(result.Unk4 == 0);

            result.Unk5 = reader.ReadByte();
            Trace.Assert(result.Unk5 == 0);

            result.Data = reader.ReadBytes(size);

            return result;
        }
    }
}
