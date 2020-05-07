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
            else
            {
                return Encoding.Unicode.GetString(buffer[0..^2]);
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
        /// <param name="chunkInfo"></param>
        public static void Write(this BinaryWriter writer, FCompressedChunkInfo chunkInfo)
        {
            writer.Write(chunkInfo.CompressedSize);
            writer.Write(chunkInfo.UncompressedSize);
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
        public static void Write(this BinaryWriter writer, FCompressedChunkHeader chunkHeader)
        {
            writer.Write(chunkHeader.PackageTag);
            writer.Write(chunkHeader.BlockSize);
            writer.Write(chunkHeader.CompressedSize);
            writer.Write(chunkHeader.UncompressedSize);
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

        public static bool IsSuspicious(this Vector3 vector)
        {
            return (vector.X > 0 && vector.X < 1E-8) || (vector.Y > 0 && vector.Y < 1E-8) || (vector.Z > 0 && vector.Z < 1E-8);
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
