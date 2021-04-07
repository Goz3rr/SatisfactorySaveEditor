using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class MapProperty : SerializedProperty
    {
        public const string TypeName = nameof(MapProperty);
        public override string PropertyType => TypeName;
        public override int SerializedLength => Data.Length + 4;

        public string KeyType { get; set; }
        public string ValueType { get; set; }

        //public Dictionary<int, (string name, string type, ArrayProperty array)> Values { get; set; } = new Dictionary<int, (string, string, ArrayProperty)>();
        public byte[] Data { get; set; }

        public MapProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            base.Serialize(writer, buildVersion);

            writer.Write(SerializedLength);
            writer.Write(Index);

            writer.WriteLengthPrefixedString(KeyType);
            writer.WriteLengthPrefixedString(ValueType);

            writer.Write((byte)0);
            writer.Write(0);

            writer.Write(Data);

            /*
            writer.Write(Values.Count);
            foreach(var kv in Values)
            {
                writer.Write(kv.Key);
                writer.WriteLengthPrefixedString(kv.Value.name);
                writer.WriteLengthPrefixedString(kv.Value.type);

                kv.Value.array.Serialize(writer);

                writer.WriteLengthPrefixedString("None");
            }
            */
        }

        public static MapProperty Parse(string propertyName, int index, BinaryReader reader, int size, out int overhead)
        {
            var result = new MapProperty(propertyName, index)
            {
                KeyType = reader.ReadLengthPrefixedString(),
                ValueType = reader.ReadLengthPrefixedString()
            };

            overhead = result.KeyType.Length + result.ValueType.Length + 11;

            var unk = reader.ReadByte();
            Trace.Assert(unk == 0);

            var unk1 = reader.ReadInt32();
            Trace.Assert(unk1 == 0);

            result.Data = reader.ReadBytes(size - 4);

            /*
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var key = reader.ReadInt32();
                var name = reader.ReadLengthPrefixedString();
                var type = reader.ReadLengthPrefixedString();

                var arr_size = reader.ReadInt32();
                var unk4 = reader.ReadInt32();
                Trace.Assert(unk4 == 0);

                var parsed = ArrayProperty.Parse(null, reader, arr_size, out int _);

                var unk5 = reader.ReadLengthPrefixedString();
                Trace.Assert(unk5 == "None");

                result.Values[key] = (name, type, parsed);
            }
            */

            return result;
        }
    }
}
