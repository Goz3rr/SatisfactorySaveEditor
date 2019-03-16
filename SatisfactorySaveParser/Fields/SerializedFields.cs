using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SatisfactorySaveParser.Fields
{
    public class SerializedFields
    {
        public static SerializedFields None = new SerializedFields();

        public List<ISerializedField> Fields { get; set; } = new List<ISerializedField>();

        public static SerializedFields Parse(int length, BinaryReader reader)
        {
            var start = reader.BaseStream.Position;
            var result = new SerializedFields();

            while (true)
            {
                var fieldName = reader.ReadLengthPrefixedString();
                if (fieldName == "None")
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
                        result.Fields.Add(ArrayProperty.Parse(fieldName, reader, size, out overhead));
                        break;
                    case "FloatProperty":
                        overhead = 1;
                        result.Fields.Add(FloatProperty.Parse(fieldName, reader));
                        break;
                    case "IntProperty":
                        overhead = 1;
                        result.Fields.Add(IntProperty.Parse(fieldName, reader));
                        break;
                    case "EnumProperty":
                        result.Fields.Add(EnumProperty.Parse(fieldName, reader, out overhead));
                        break;
                    case "BoolProperty":
                        overhead = 2;
                        result.Fields.Add(BoolProperty.Parse(fieldName, reader));
                        break;
                    case "StrProperty":
                        overhead = 1;
                        result.Fields.Add(StrProperty.Parse(fieldName, reader));
                        break;
                    case "NameProperty":
                        overhead = 1;
                        result.Fields.Add(NameProperty.Parse(fieldName, reader));
                        break;
                    case "ObjectProperty":
                        overhead = 1;
                        result.Fields.Add(ObjectProperty.Parse(fieldName, reader));
                        break;
                    case "StructProperty":
                        result.Fields.Add(StructProperty.Parse(fieldName, reader, size, out overhead));
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
