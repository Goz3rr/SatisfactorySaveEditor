using System;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class ArrayProperty : SerializedProperty
    {
        public const string TypeName = nameof(ArrayProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(Array);
        public override object BackingObject => null;

        public override int SerializedLength => 0;

        public static ArrayProperty Parse(BinaryReader reader, string propertyName, int size, int index, out int overhead)
        {
            var type = reader.ReadLengthPrefixedString();
            reader.ReadByte();

            overhead = type.GetSerializedLength() + 1;

            reader.ReadBytes(size);
            return new ArrayProperty()
            {
                PropertyName = propertyName
            };
        }
    }
}
