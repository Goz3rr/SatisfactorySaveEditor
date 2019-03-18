using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class TextProperty : SerializedProperty
    {
        public const string TypeName = nameof(TextProperty);

        public List<string> Values { get; set; } = new List<string>();

        public TextProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"text";
        }

        public static TextProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new TextProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            var count = reader.ReadInt32();
            var unk4 = reader.ReadInt32();
            var unk6 = reader.ReadByte();

            for(int i = 0; i < count; i++)
            {
                result.Values.Add(reader.ReadLengthPrefixedString());
            }

            return result;
        }
    }
}
