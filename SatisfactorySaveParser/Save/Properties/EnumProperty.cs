using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SatisfactorySaveParser.Save.Properties
{
    public class EnumProperty : SerializedProperty
    {
        public const string TypeName = nameof(EnumProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(Enum);
        public override object BackingObject => Value;

        public override int SerializedLength => 4;

        public int Value { get; set; }


        public override string ToString()
        {
            return $"Int {PropertyName}: {Value}";
        }

        public static EnumProperty Parse(BinaryReader reader, string propertyName, int index)
        {
            var result = new EnumProperty()
            {
                PropertyName = propertyName,
                Index = index
            };

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            result.Value = reader.ReadInt32();
            return result;
        }
    }
}
