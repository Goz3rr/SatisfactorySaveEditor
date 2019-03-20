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

        public List<string> Values { get; set; } = new List<string>();

        public TextProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"text";
        }

        public override void Serialize(BinaryWriter writer, bool writeHeader = true)
        {
            base.Serialize(writer, writeHeader);

            writer.Write(4 + 5 + Values.Sum(v => v.GetSerializedLength()));
            writer.Write(0);
            writer.Write((byte)0);

            writer.Write(Values.Count);

            writer.Write(0);
            writer.Write((byte)0);

            foreach(var str in Values)
            {
                writer.WriteLengthPrefixedString(str);
            }
        }

        public static TextProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new TextProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            var count = reader.ReadInt32();

            var unk4 = reader.ReadInt32();
            Trace.Assert(unk4 == 0);
            var unk6 = reader.ReadByte();
            Trace.Assert(unk6 == 0);

            for(int i = 0; i < count; i++)
            {
                result.Values.Add(reader.ReadLengthPrefixedString());
            }

            return result;
        }
    }
}
