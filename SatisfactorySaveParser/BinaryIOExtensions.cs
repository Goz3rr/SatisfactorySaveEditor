using SatisfactorySaveParser.Data;
using System.IO;
using System.Text;

namespace SatisfactorySaveParser
{
    public static class BinaryIOExtensions
    {
        public static char[] ReadCharArray(this BinaryReader reader)
        {
            var count = reader.ReadInt32();
            return reader.ReadChars(count);
        }

        public static string ReadLengthPrefixedString(this BinaryReader reader)
        {
            return new string(reader.ReadCharArray()).TrimEnd('\0');
        }

        public static void WriteLengthPrefixedString(this BinaryWriter writer, string str)
        {
            if (str == null) return;

            var bytes = Encoding.ASCII.GetBytes(str);
            if (bytes.Length > 0)
            {
                writer.Write(bytes.Length + 1);
                writer.Write(bytes);
                writer.Write((byte)0);
            }
            else
            {
                writer.Write(0);
            }
        }

        public static int GetSerializedLength(this string str)
        {
            if (str == null || str.Length == 0) return 4;

            return str.Length + 5;
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3() {
                X = reader.ReadSingle(),
                Y = reader.ReadSingle(),
                Z = reader.ReadSingle()
            };
        }

        public static void Write(this BinaryWriter writer, Vector3 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
        }

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

        public static void Write(this BinaryWriter writer, Vector4 vec)
        {
            writer.Write(vec.X);
            writer.Write(vec.Y);
            writer.Write(vec.Z);
            writer.Write(vec.W);
        }
    }
}
