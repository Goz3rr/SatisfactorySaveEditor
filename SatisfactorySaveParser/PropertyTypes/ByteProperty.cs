using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ByteProperty : SerializedProperty
    {
        public const string TypeName = nameof(ByteProperty);

        public string Type { get; set; }
        public string Value { get; set; }

        public ByteProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"byte";
        }

        public static ByteProperty Parse(string propertyName, BinaryReader reader, out int overhead)
        {
            var result = new ByteProperty(propertyName)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            var unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            if (result.Type == "None")
            {
                result.Value = reader.ReadByte().ToString();
            }
            else
            {
                result.Value = reader.ReadLengthPrefixedString();
            }

            overhead = result.Type.Length + 6;

            return result;
        }
    }
}
