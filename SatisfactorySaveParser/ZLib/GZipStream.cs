using System.IO;

namespace SatisfactorySaveParser.ZLib
{
    public class GZipStream : DeflateStream
    {
        protected override Format OpenType => Format.GZip;
        protected override Format WriteType => Format.GZip;

        public GZipStream(Stream stream, CompressionMode mode)
            : base(stream, mode)
        {
        }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) :
            base(stream, mode, level)
        {
        }
    }
}
