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

            var length = count > 0 ? count : count * -2;
            var buffer = length <= 1024 ? stackalloc byte[length] : new byte[length];

            var read = reader.Read(buffer);
            if (buffer[read - 1] != 0)
                throw new FatalSaveException($"Read non null terminated string", reader.BaseStream.Position - length);

            // If count is negative the string is UTF16, otherwise it's ASCII
            if (count > 0)
            {
                return Encoding.ASCII.GetString(buffer[0..^1]);
            }

            return Encoding.Unicode.GetString(buffer[0..^2]);
        }

        /// <summary>
        ///     Write a length prefixed ASCII or UTF16 string
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void WriteLengthPrefixedString(this BinaryWriter writer, string value)
        {
            if (value == null || value.Length == 0)
            {
                writer.Write(0);
                return;
            }

            if (value.Any(c => c > 127))
            {
                var bytes = Encoding.Unicode.GetBytes(value);

                writer.Write(-(bytes.Length / 2 + 1));
                writer.Write(bytes);
                writer.Write((short)0);
            }
            else
            {
                var bytes = Encoding.ASCII.GetBytes(value);

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
        ///     Read a single Vector2 (X, Y)
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Vector2 ReadVector2(this BinaryReader reader)
        {
            return new Vector2()
            {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
            };
        }

        /// <summary>
        ///     Write a single Vector2
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, Vector2 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
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
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, Vector3 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
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
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, Vector4 value)
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
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
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, ObjectReference value)
        {
            writer.WriteLengthPrefixedString(value.LevelName);
            writer.WriteLengthPrefixedString(value.PathName);
        }

        /// <summary>
        ///     Read an UE4 FCompressedChunkInfo
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static FCompressedChunkInfo ReadCompressedChunkInfo(this BinaryReader reader)
        {
            return new FCompressedChunkInfo()
            {
                CompressedSize = reader.ReadInt64(),
                UncompressedSize = reader.ReadInt64()
            };
        }

        /// <summary>
        ///     Write an UE4 FCompressedChunkInfo
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, FCompressedChunkInfo value)
        {
            writer.Write(value.CompressedSize);
            writer.Write(value.UncompressedSize);
        }

        /// <summary>
        ///     Read an UE4 FCompressedChunkHeader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static FCompressedChunkHeader ReadCompressedChunkHeader(this BinaryReader reader)
        {
            return new FCompressedChunkHeader()
            {
                PackageTag = reader.ReadInt64(),
                BlockSize = reader.ReadInt64(),
                CompressedSize = reader.ReadInt64(),
                UncompressedSize = reader.ReadInt64()
            };
        }

        /// <summary>
        ///     Write an UE4 FCompressedChunkHeader
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="chunkInfo"></param>
        public static void Write(this BinaryWriter writer, FCompressedChunkHeader value)
        {
            writer.Write(value.PackageTag);
            writer.Write(value.BlockSize);
            writer.Write(value.CompressedSize);
            writer.Write(value.UncompressedSize);
        }

        /// <summary>
        ///     Read a 16 byte GUID
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Guid ReadGuid(this BinaryReader reader)
        {
            return new Guid(reader.ReadBytes(16));
        }

        /// <summary>
        ///     Write a GUID
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void Write(this BinaryWriter writer, Guid value)
        {
            writer.Write(value.ToByteArray());
        }

        /// <summary>
        ///     Reads a bool from 4 bytes
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>True for all nonzero values</returns>
        public static bool ReadBooleanFromInt32(this BinaryReader reader)
        {
            return reader.ReadInt32() != 0;
        }

        /// <summary>
        ///     Write bool as 4 byte int (0 or 1)
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        public static void WriteBoolAsInt32(this BinaryWriter writer, bool value)
        {
            writer.Write(value ? 1 : 0);
        }

        public static bool IsSuspicious(this Vector3 vector)
        {
            return Math.Abs(vector.X) < 1E-8 || Math.Abs(vector.Y) < 1E-8 || Math.Abs(vector.Z) < 1E-8;
        }

        public static void AssertNullByte(this BinaryReader reader)
        {
            var nullByte = reader.ReadByte();
            Trace.Assert(nullByte == 0, $"Expected null byte, got {nullByte} instead at {reader.BaseStream.Position - 1}");
        }

        public static void AssertNullInt32(this BinaryReader reader)
        {
            var nullInt = reader.ReadInt32();
            Trace.Assert(nullInt == 0, $"Expected null int32, got {nullInt} instead at {reader.BaseStream.Position - 4}");
        }
    }
}
