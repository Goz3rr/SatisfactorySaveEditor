using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.Save.Properties
{
    public class TextProperty : SerializedProperty
    {
        public const string TypeName = nameof(TextProperty);
        public override string PropertyType => TypeName;

        public override Type BackingType => typeof(TextEntry);
        public override object BackingObject => Text;

        public override int SerializedLength => 0;

        public TextEntry Text { get; set; }

        public TextProperty(string propertyName, int index = 0) : base(propertyName, index)
        {
        }

        public override string ToString()
        {
            return $"Text {PropertyName}: {Text}";
        }

        public static TextProperty Deserialize(BinaryReader reader, string propertyName, int index)
        {
            var result = new TextProperty(propertyName, index);
            result.Text = new TextEntry();

            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0);

            result.Text.UnknownInt = reader.ReadInt32();

            var nullByte2 = reader.ReadByte();
            Trace.Assert(nullByte2 == 0);

            var nullInt = reader.ReadInt32();
            Trace.Assert(nullInt == 0);

            result.Text.UnknownString = reader.ReadLengthPrefixedString();
            result.Text.Value = reader.ReadLengthPrefixedString();

            return result;
        }
    }
}
