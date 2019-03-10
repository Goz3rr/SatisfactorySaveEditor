using System.IO;

namespace SatisfactorySaveParser
{
    public static class BinaryReaderExtensions
    {
        public static char[] ReadCharArray(this BinaryReader reader)
        {
            return reader.ReadChars(reader.ReadInt32());
        }

        public static string ReadLengthPrefixedString(this BinaryReader reader)
        {
            return new string(reader.ReadCharArray()).TrimEnd('\0');
        }
    }
}
