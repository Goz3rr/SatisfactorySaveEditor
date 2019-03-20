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
    }
}
