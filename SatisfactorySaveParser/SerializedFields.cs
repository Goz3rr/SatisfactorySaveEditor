using NLog;
using SatisfactorySaveParser.PropertyTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser
{
    public class SerializedFields : List<SerializedProperty>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     Used to handle edge cases where objects have 0 bytes of data, and we don't want to generate a "None" string either
        /// </summary>
        public bool ShouldBeNulled { get; private set; } = false;

        public byte[] TrailingData { get; set; }

        public void Serialize(BinaryWriter writer, int buildVersion)
        {
            if (ShouldBeNulled && Count == 0 && TrailingData.Length == 0)
                return;

            foreach (var field in this)
            {
                field.Serialize(writer, buildVersion);
            }

            writer.WriteLengthPrefixedString("None");

            writer.Write(0);
            if (TrailingData != null)
                writer.Write(TrailingData);
        }

        public static SerializedFields Parse(int length, BinaryReader reader, int buildVersion)
        {
            var start = reader.BaseStream.Position;
            var result = new SerializedFields();

            if (length == 0)
            {
                log.Warn($"Tried to parse 0 byte object data @ {start}");
                result.ShouldBeNulled = true;
                return result;
            }

            SerializedProperty prop;
            while ((prop = SerializedProperty.Parse(reader, buildVersion)) != null)
            {
                result.Add(prop);
            }

            var int1 = reader.ReadInt32();
            Trace.Assert(int1 == 0);

            var remainingBytes = start + length - reader.BaseStream.Position;
            if (remainingBytes > 0)
            {
                //log.Warn($"{remainingBytes} bytes left after reading all serialized fields!");
                result.TrailingData = reader.ReadBytes((int)remainingBytes);
                //log.Trace(BitConverter.ToString(result.TrailingData).Replace("-", " "));
            }

            //if (remainingBytes == 4)
            ////if(result.Fields.Count > 0)
            //{
            //    var int2 = reader.ReadInt32();
            //}
            //else if (remainingBytes > 0 && result.Any(f => f is ArrayProperty && ((ArrayProperty)f).Type == StructProperty.TypeName))
            //{
            //    var unk = reader.ReadBytes((int)remainingBytes);
            //}
            //else if (remainingBytes > 4)
            //{
            //    var int2 = reader.ReadInt32();
            //    var str2 = reader.ReadLengthPrefixedString();
            //    var str3 = reader.ReadLengthPrefixedString();
            //}


            return result;
        }
    }
}
