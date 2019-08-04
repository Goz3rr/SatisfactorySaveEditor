using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class MapProperty : SerializedProperty
    {
        public const string TypeName = nameof(MapProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(Dictionary<,>);
        public override object BackingObject => null;

        public override int SerializedLength => 0;

        public string KeyType { get; set; }
        public string ValueType { get; set; }

        public MapProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static MapProperty Deserialize(BinaryReader reader, string propertyName, int size, int index, out int overhead)
        {
            var result = new MapProperty(propertyName, index)
            {
                KeyType = reader.ReadLengthPrefixedString(),
                ValueType = reader.ReadLengthPrefixedString()
            };

            overhead = result.KeyType.Length + result.ValueType.Length + 11;

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            var unk1 = reader.ReadInt32();
            var data = reader.ReadBytes(size - 4);

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
