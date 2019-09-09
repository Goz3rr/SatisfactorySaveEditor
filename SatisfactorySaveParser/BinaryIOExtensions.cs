using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

using SatisfactorySaveParser.Save;

namespace SatisfactorySaveParser
{
    public static class BinaryIOExtensions
    {
        /// <summary>
        ///     Read a length prefixed ASCII or UTF16 string
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static string ReadLengthPrefixedString(this BinaryReader reader)
        {
            var count = reader.ReadInt32();
            if (count == 0) return String.Empty;

            // If count is negative the string is UTF16, otherwise it's ASCII
            if (count > 0)
            {
                var bytes = reader.ReadBytes(count);
                if (bytes.Last() != 0)
                    throw new FatalSaveException($"Read non null terminated string", reader.BaseStream.Position - count);

                return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
            }
            else
            {
                var bytes = reader.ReadBytes(count * -2);
                if (bytes.Last() != 0)
                    throw new FatalSaveException($"Read non null terminated string", reader.BaseStream.Position - count);

                return Encoding.Unicode.GetString(bytes).TrimEnd('\0');
            }
        }

        /// <summary>
        ///     Write a length prefixed ASCII or UTF16 string
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="str"></param>
        public static void WriteLengthPrefixedString(this BinaryWriter writer, string str)
        {
            if (str == null || str.Length == 0)
            {
                writer.Write(0);
                return;
            }

            if (str.Any(c => c > 127))
            {
                var bytes = Encoding.Unicode.GetBytes(str);

                writer.Write(-(bytes.Length / 2 + 1));
                writer.Write(bytes);
                writer.Write((short)0);
            }
            else
            {
                var bytes = Encoding.ASCII.GetBytes(str);

                writer.Write(bytes.Length + 1);
                writer.Write(bytes);
                writer.Write((byte)0);
            }
        }

        /// <summary>
        ///     Get the serialized length of a string in bytes, while taking encoding and overhead into account
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetSerializedLength(this string str)
        {
            if (str == null || str.Length == 0) return 4;

            if (str.Any(c => c > 127))
            {
                return str.Length * 2 + 6;
            }

            return str.Length + 5;
        }

        /// <summary>
        ///     Read a single Vector3 (X, Y, Z)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3()
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle()
            };
        }

        /// <summary>
        ///     Write a single Vector3
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="vec"></param>
        public static void Write(this BinaryWriter writer, Vector3 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }


        /// <summary>
        ///     Read a single Vector4 (X, Y, Z, W)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector4 ReadVector4(this BinaryReader reader)
        {
            return new Vector4()
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle(),
                W = reader.ReadSingle()
            };
        }

        /// <summary>
        ///     Write a single Vector4
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="vec"></param>
        public static void Write(this BinaryWriter writer, Vector4 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }

        /// <summary>
        ///     Read a UE4 Object Reference (Level, Path)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static ObjectReference ReadObjectReference(this BinaryReader reader)
        {
            return new ObjectReference(level: reader.ReadLengthPrefixedString(), path: reader.ReadLengthPrefixedString());
        }

        /// <summary>
        ///     Write a UE4 Object Reference (Level, Path)
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="objectReference"></param>
        public static void Write(this BinaryWriter writer, ObjectReference objectReference)
        {
            writer.WriteLengthPrefixedString(objectReference.LevelName);
            writer.WriteLengthPrefixedString(objectReference.PathName);
        }

        /// <summary>
        ///     Get the serialized length of an ObjectReference in bytes
        /// </summary>
        /// <param name="objRef"></param>
        /// <returns></returns>
        public static int GetSerializedLength(this ObjectReference objRef)
        {
            return objRef.LevelName.GetSerializedLength() + objRef.PathName.GetSerializedLength();
        }

        public static bool IsSuspicious(this Vector3 vector)
        {
            return vector.X < 1E-8 || vector.Y < 1E-8 || vector.Y < 1E-8;
        }

        public static void AssertNullByte(this BinaryReader reader)
        {
            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0, $"Expected null byte, got {nullByte} instead at {reader.BaseStream.Position - 1}");
        }

        public static void AssertNullInt32(this BinaryReader reader)
        {
            var nullInt = reader.ReadInt32();
            Trace.Assert(nullInt == 0, $"Expected null int32, got {nullInt} instead at {reader.BaseStream.Position - 1}");
        }
    }
}
