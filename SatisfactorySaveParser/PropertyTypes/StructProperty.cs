using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class StructProperty : SerializedProperty
    {
        public const string TypeName = nameof(StructProperty);
        public override string PropertyType => TypeName;

        public string Type { get; set; }
        public int Unk1 { get; set; }
        public int Unk2 { get; set; }
        public int Unk3 { get; set; }
        public int Unk4 { get; set; }
        public byte Unk5 { get; set; }

        public byte[] Data { get; set; }

        public StructProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"struct {Type}";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(Data.Length);
            writer.Write(0);

            writer.WriteLengthPrefixedString(Type);
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(Unk3);
            writer.Write(Unk4);
            writer.Write(Unk5);
            writer.Write(Data);
        }

        public static StructProperty Parse(string propertyName, BinaryReader reader, int size, out int overhead)
        {
            var result = new StructProperty(propertyName)
            {
                Type = reader.ReadLengthPrefixedString()
            };

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
