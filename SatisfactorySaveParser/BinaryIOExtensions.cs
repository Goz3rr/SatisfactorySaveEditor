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
            var bytes = Encoding.ASCII.GetBytes(str);
            writer.Write(bytes.Length + 1);
            writer.Write(bytes);
            writer.Write((byte)0);
        }
    }
}
