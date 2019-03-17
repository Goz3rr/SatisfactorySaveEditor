using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ByteProperty : SerializedProperty
    {
        public const string TypeName = nameof(ByteProperty);

        public ByteProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"byte";
        }

        public static ByteProperty Parse(string propertyName, BinaryReader reader, out int overhead)
        {
            var str1 = reader.ReadLengthPrefixedString();
            var unk = reader.ReadByte();
            if (str1 == "None")
            {
                var unk2 = reader.ReadByte();
            }
            else
            {
                var str2 = reader.ReadLengthPrefixedString();
            }

            overhead = str1.Length + 6;

            return new ByteProperty(propertyName);
        }
    }
}
