using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class Int64Property : SerializedProperty
    {
        public const string TypeName = nameof(Int64Property);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(long);
        public override object BackingObject => Value;

        public override int SerializedLength => 8;

        public long Value { get; set; }

        public Int64Property(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Int64 {PropertyName}: {Value}";
        }

        public static Int64Property Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new Int64Property(propertyName, index);

            reader.AssertNullByte();
            result.Value = reader.ReadInt64();

            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)0);
            writer.Write(Value);
        }
    }
}
