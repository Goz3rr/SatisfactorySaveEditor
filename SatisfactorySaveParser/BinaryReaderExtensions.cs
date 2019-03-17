using System.IO;

namespace SatisfactorySaveParser
{
    public static class BinaryReaderExtensions
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
    }
}
