using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class TextProperty : SerializedProperty
    {
        public const string TypeName = nameof(TextProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength
        {
            get
            {
                var size = 9 + Unknown8.GetSerializedLength() + Value.GetSerializedLength() + FormatData.Sum(d => d.SerializedLength);
                if (Unknown5 == 3)
                    size += 9;

                return size;
            }
        }

        public int Unknown4 { get; set; }
        public byte Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public byte Unknown7 { get; set; }
        public string Unknown8 { get; set; }
        public string Value { get; set; }

        public List<TextFormatData> FormatData { get; } = new List<TextFormatData>();

        public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"text";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(SerializedLength);
            writer.Write(Index);
            writer.Write((byte)0);

            writer.Write(Unknown4);

            writer.Write(Unknown5);
            if (Unknown5 == 3)
            {
                writer.Write(Unknown6);
                writer.Write(Unknown7);
            }

            writer.Write(0);

            writer.WriteLengthPrefixedString(Unknown8);
            writer.WriteLengthPrefixedString(Value);

            if (Unknown5 == 3)
            {
                writer.Write(FormatData.Count);
                foreach (var formatData in FormatData)
                {
                    writer.WriteLengthPrefixedString(formatData.Name);
                    writer.Write(formatData.Unknown);
                    writer.Write(formatData.Data.Length);
                    writer.Write(formatData.Data);
                }
            }
        }

        public static TextProperty Parse(string propertyName, int index, BinaryReader reader)
        {
            var result = new TextProperty(propertyName, index);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Unknown4 = reader.ReadInt32();

            result.Unknown5 = reader.ReadByte();
            if (result.Unknown5 == 3)
            {
                result.Unknown6 = reader.ReadInt32();
                result.Unknown7 = reader.ReadByte();
            }

            var unk5 = reader.ReadInt32();
            Trace.Assert(unk5 == 0);

            result.Unknown8 = reader.ReadLengthPrefixedString();

            result.Value = reader.ReadLengthPrefixedString();

            if (result.Unknown5 == 3)
            {
                var count = reader.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var formatName = reader.ReadLengthPrefixedString();
                    var formatUnknown = reader.ReadByte();
                    var formatLength = reader.ReadInt32();
                    var formatBytes = reader.ReadBytes(formatLength);

                    result.FormatData.Add(new TextFormatData()
                    {
                        Name = formatName,
                        Unknown = formatUnknown,
                        Data = formatBytes
                    }
                    );
                }
            }

            return result;
        }
    }

    public class TextFormatData
    {
        public int SerializedLength => Name.GetSerializedLength() + 5 + Data.Length;

        public string Name { get; set; }
        public byte Unknown { get; set; }
        public byte[] Data { get; set; }
    }
}
