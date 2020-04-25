namespace SatisfactorySaveParser.Save
{
    public class FCompressedChunkHeader
    {
        public const long Magic = 0x9E2A83C1;
        public const int ChunkSize = 131072; // 128 KiB

        public long PackageTag { get; set; }
        public long BlockSize { get; set; }

        public long CompressedSize { get; set; }
        public long UncompressedSize { get; set; }
    }
}
