using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public class FloatProperty : SerializedProperty
    {
        public float Value { get; set; }

        public FloatProperty(string propertyName) : base(propertyName)
        {
        }

        public override string ToString()
        {
            return $"float: {Value}";
        }

        public static FloatProperty Parse(string propertyName, BinaryReader reader)
        {
            var result = new FloatProperty(propertyName);

            var unk3 = reader.ReadByte();
            Trace.Assert(unk3 == 0);

            result.Value = reader.ReadSingle();

            return result;
        }
    }
}
