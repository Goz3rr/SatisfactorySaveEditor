using System;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser.PropertyTypes
{
    public abstract class SerializedProperty
    {
        public string PropertyName { get; }
        public abstract string PropertyType { get; }
        public int Index { get; }

        public abstract int SerializedLength { get; }

        protected SerializedProperty(string propertyName, int index)
        {
            PropertyName = propertyName;
            Index = index;
        }

        public virtual void Serialize(BinaryWriter writer, int buildVersion, bool writeHeader = true)
        {
            writer.WriteLengthPrefixedString(PropertyName);
            writer.WriteLengthPrefixedString(PropertyType);
        }

        public static SerializedProperty Parse(BinaryReader reader, int buildVersion)
        {
            SerializedProperty result;

            var propertyName = reader.ReadLengthPrefixedString();
            if (propertyName == "None")
            {
                return null;
            }

            Trace.Assert(!String.IsNullOrEmpty(propertyName));

            var fieldType = reader.ReadLengthPrefixedString();
            var size = reader.ReadInt32();
            var index = reader.ReadInt32();

            int overhead;
            var before = reader.BaseStream.Position;
            switch (fieldType)
            {
                case ArrayProperty.TypeName:
                    result = ArrayProperty.Parse(propertyName, index, reader, size, out overhead, buildVersion);
                    break;
                case FloatProperty.TypeName:
                    overhead = 1;
                    result = FloatProperty.Parse(propertyName, index, reader);
                    break;
                case IntProperty.TypeName:
                    overhead = 1;
                    result = IntProperty.Parse(propertyName, index, reader);
                    break;
                case ByteProperty.TypeName:
                    result = ByteProperty.Parse(propertyName, index, reader, out overhead);
                    break;
                case EnumProperty.TypeName:
                    result = EnumProperty.Parse(propertyName, index, reader, out overhead);
                    break;
                case BoolProperty.TypeName:
                    overhead = 2;
                    result = BoolProperty.Parse(propertyName, index, reader);
                    break;
                case StrProperty.TypeName:
                    overhead = 1;
                    result = StrProperty.Parse(propertyName, index, reader);
                    break;
                case NameProperty.TypeName:
                    overhead = 1;
                    result = NameProperty.Parse(propertyName, index, reader);
                    break;
                case ObjectProperty.TypeName:
                    overhead = 1;
                    result = ObjectProperty.Parse(propertyName, index, reader);
                    break;
                case StructProperty.TypeName:
                    result = StructProperty.Parse(propertyName, index, reader, size, out overhead, buildVersion);
                    break;
                case MapProperty.TypeName:
                    result = MapProperty.Parse(propertyName, index, reader, size, out overhead);
                    break;
                case TextProperty.TypeName:
                    overhead = 1;
                    result = TextProperty.Parse(propertyName, index, reader, buildVersion);
                    break;
                case SetProperty.TypeName:
                    result = SetProperty.Parse(propertyName, index, reader, size, out overhead);
                    break;
                case Int64Property.TypeName:
                    overhead = 1;
                    result = Int64Property.Parse(propertyName, index, reader);
                    break;
                case Int8Property.TypeName:
                    overhead = 1;
                    result = Int8Property.Parse(propertyName, index, reader);
                    break;
                case InterfaceProperty.TypeName:
                    overhead = 1;
                    result = InterfaceProperty.Parse(propertyName, index, reader);
                    break;
                default:
                    throw new NotImplementedException(fieldType);
            }

            var after = reader.BaseStream.Position;
            var readBytes = (int)(after - before - overhead);

            if (size != readBytes)
            {
                throw new InvalidOperationException($"Expected {size} bytes read but got {readBytes}");
            }

            return result;
        }
    }
}