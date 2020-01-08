using System.IO;

namespace SatisfactorySaveParser.Save
{
    public class ChunkInfo
    {
        public const long Magic = 0x9E2A83C1;
        public const int ChunkSize = 131072; // 128 KiB

        public long CompressedSize { get; set; }
        public long UncompressedSize { get; set; }
    }
}
