using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SatisfactorySaveParser.Save.Properties
{
    public class BoolProperty : SerializedProperty
    {
        public const string TypeName = nameof(BoolProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(bool);
        public override object BackingObject => Value;

        public override int SerializedLength => 0;

        public bool Value { get; set; }


        public override string ToString()
        {
            return $"Bool {PropertyName}: {Value}";
        }

        public static BoolProperty Parse(BinaryReader reader, string propertyName, int index)
        {
            var result = new BoolProperty()
            {
                PropertyName = propertyName,
                Index = index
            };

            result.Value = reader.ReadByte() > 0;

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            return result;
        }
    }
}
