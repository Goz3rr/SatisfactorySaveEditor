using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class MapProperty : SerializedProperty
    {
        public const string TypeName = nameof(MapProperty);

        public string KeyType { get; set; }
        public string ValueType { get; set; }

        public Dictionary<int, ArrayProperty> Values = new Dictionary<int, ArrayProperty>();

        public MapProperty(string propertyName) : base(propertyName)
        {
        }

        public static MapProperty Parse(string propertyName, BinaryReader reader, int size, out int overhead)
        {
            var result = new MapProperty(propertyName);

            result.KeyType = reader.ReadLengthPrefixedString();
            result.ValueType = reader.ReadLengthPrefixedString();
            overhead = result.KeyType.Length + result.ValueType.Length + 11;

            var unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            var unk1 = reader.ReadInt32();
            Trace.Assert(unk1 == 0);

            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.ReadInt32();
                var name = reader.ReadLengthPrefixedString();
                var type = reader.ReadLengthPrefixedString();

                var arr_size = reader.ReadInt32();
                var unk4 = reader.ReadInt32();
                Trace.Assert(unk4 == 0);

                var parsed = ArrayProperty.Parse("foo", reader, arr_size, out int _);

                var unk5 = reader.ReadLengthPrefixedString();
                Trace.Assert(unk5 == "None");

                result.Values[key] = parsed;
            }

            return result;
        }
    }
}
