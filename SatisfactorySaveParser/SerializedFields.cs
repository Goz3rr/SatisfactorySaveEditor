using SatisfactorySaveParser.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser
{
    public class SerializedFields
    {
        public static SerializedFields None = new SerializedFields();

        public List<SerializedProperty> Fields { get; set; } = new List<SerializedProperty>();

        public static SerializedFields Parse(int length, BinaryReader reader)
        {
            var start = reader.BaseStream.Position;
            var result = new SerializedFields();

            while (true)
            {
                var propertyName = reader.ReadLengthPrefixedString();
                if (propertyName == "None")
                {
                    break;
                }

                var fieldType = reader.ReadLengthPrefixedString();
                var size = reader.ReadInt32();

                var unk = reader.ReadInt32();
                Trace.Assert(unk == 0);

                var overhead = 0;
                var before = reader.BaseStream.Position;
                switch (fieldType)
                {
                    case "ArrayProperty":
                        result.Fields.Add(ArrayProperty.Parse(propertyName, reader, size, out overhead));
                        break;
                    case "FloatProperty":
                        overhead = 1;
                        result.Fields.Add(FloatProperty.Parse(propertyName, reader));
                        break;
                    case "IntProperty":
                        overhead = 1;
                        result.Fields.Add(IntProperty.Parse(propertyName, reader));
                        break;
                    case "EnumProperty":
                        result.Fields.Add(EnumProperty.Parse(propertyName, reader, out overhead));
                        break;
                    case "BoolProperty":
                        overhead = 2;
                        result.Fields.Add(BoolProperty.Parse(propertyName, reader));
                        break;
                    case "StrProperty":
                        overhead = 1;
                        result.Fields.Add(StrProperty.Parse(propertyName, reader));
                        break;
                    case "NameProperty":
                        overhead = 1;
                        result.Fields.Add(NameProperty.Parse(propertyName, reader));
                        break;
                    case "ObjectProperty":
                        overhead = 1;
                        result.Fields.Add(ObjectProperty.Parse(propertyName, reader));
                        break;
                    case "StructProperty":
                        result.Fields.Add(StructProperty.Parse(propertyName, reader, size, out overhead));
                        break;
                    default:
                        throw new NotImplementedException(fieldType);
                }
                var after = reader.BaseStream.Position;

                if (before + size + overhead != after)
                {
                    throw new InvalidOperationException($"Expected {size} bytes read but got {after - before - overhead}");
                }
            }

            var int1 = reader.ReadInt32();
            Trace.Assert(int1 == 0);

            var remainingBytes = start + length - reader.BaseStream.Position;
            if (remainingBytes == 4)
            //if(result.Fields.Count > 0)
            {
                var int2 = reader.ReadInt32();
            }
            else if (remainingBytes > 0 && result.Fields.Any(f => f is ArrayProperty && ((ArrayProperty)f).Type == "StructProperty"))
            {
                var unk = reader.ReadBytes((int)remainingBytes);
            }
            else if (remainingBytes > 4)
            {
                var int2 = reader.ReadInt32();
                var str2 = reader.ReadLengthPrefixedString();
                var str3 = reader.ReadLengthPrefixedString();
            }


            return result;
        }
    }
}
