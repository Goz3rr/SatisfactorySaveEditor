using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

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

            // If count is negative the string is UTF16, otherwise it's ASCII
            if (count >= 0)
            {
                var bytes = reader.ReadBytes(count);
                return Encoding.ASCII.GetString(bytes).TrimEnd('\0');
            }
            else
            {
                var bytes = reader.ReadBytes(count * -2);
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
    }
}
