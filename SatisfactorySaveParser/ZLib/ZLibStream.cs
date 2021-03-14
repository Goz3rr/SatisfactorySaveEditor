using System.IO;

namespace SatisfactorySaveParser.ZLib
{
    public class ZLibStream : DeflateStream
    {
        protected override Format OpenType => Format.ZLib;
        protected override Format WriteType => Format.ZLib;

        public ZLibStream(Stream stream, CompressionMode mode)
            : base(stream, mode)
        {
        }

        public ZLibStream(Stream stream, CompressionMode mode, CompressionLevel level) :
            base(stream, mode, level)
        {
        }
    }
}
