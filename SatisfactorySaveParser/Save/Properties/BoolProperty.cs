using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class BoolProperty : SerializedProperty, IBoolPropertyValue
    {
        public const string TypeName = nameof(BoolProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(bool);
        public override object BackingObject => Value;

        public override int SerializedLength => 0;

        public bool Value { get; set; }

        public BoolProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Bool {PropertyName}: {Value}";
        }

        public static BoolProperty Deserialize(BinaryReader reader, string propertyName, int index, out int overhead)
        {
            var result = new BoolProperty(propertyName, index)
            {
                Value = reader.ReadByte() != 0
            };

            reader.AssertNullByte();

            overhead = 2;
            return result;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)(Value ? 1 : 0));
            writer.Write((byte)0);
        }
    }
}
