using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class ObjectProperty : SerializedProperty
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }

        public ObjectProperty(string propertyName, string str1 = null, string str2 = null) : base(propertyName)
        {
            Str1 = str1;
            Str2 = str2;
        }


        public override string ToString()
        {
            return $"obj: {Str2}";
        }

        public static ObjectProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new ObjectProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Str1 = reader.ReadLengthPrefixedString();
            result.Str2 = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
