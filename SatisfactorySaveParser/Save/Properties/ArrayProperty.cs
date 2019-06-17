using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        ///     String representation of the Property type this array consists of
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Actual content of the array
        /// </summary>
        public List<SerializedProperty> Elements { get; } = new List<SerializedProperty>();

        public ArrayProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public static ArrayProperty Parse(BinaryReader reader, string propertyName, int size, int index, out int overhead)
        {
            var result = new ArrayProperty(propertyName, index)
            {
                Type = reader.ReadLengthPrefixedString()
            };

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            overhead = result.Type.GetSerializedLength() + 1;

            reader.ReadBytes(size);

            return result;
        }
    }
}
