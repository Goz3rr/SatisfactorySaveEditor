using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class NameProperty : SerializedProperty
    {
        public string Value { get; set; }

        public NameProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"name: {Value}";
        }

        public static NameProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new NameProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
