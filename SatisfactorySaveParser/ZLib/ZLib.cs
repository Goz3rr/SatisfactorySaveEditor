#pragma warning disable CA1069 // Enums values should not be duplicated

using System;
using System.Runtime.InteropServices;

namespace SatisfactorySaveParser.ZLib
{
#pragma warning disable IDE1006 // Naming Styles
    [StructLayout(LayoutKind.Sequential)]
    internal struct z_streamp
    {
        /// <summary>
        /// next input byte
        /// </summary>
        public IntPtr next_in;

        /// <summary>
        /// number of bytes available at next_in
        /// </summary>
        public uint avail_in;

        /// <summary>
        /// total number of input bytes read so far
        /// </summary>
        public uint total_in;

        /// <summary>
        /// next output byte will go here
        /// </summary>
        public IntPtr next_out;

        /// <summary>
        /// remaining free space at next_out
        /// </summary>
        public uint avail_out;

        /// <summary>
        /// total number of bytes output so far
        /// </summary>
        public uint total_out;

        private readonly IntPtr msg;

        /// <summary>
        /// last error message, NULL if no error
        /// </summary>
        public string LastErrorMessage => Marshal.PtrToStringAnsi(msg);

        /// <summary>
        /// not visible by applications 
        /// </summary>
        private readonly IntPtr state;

        /// <summary>
        /// used to allocate the internal state
        /// </summary>
        private readonly IntPtr zalloc;

        /// <summary>
        /// used to free the internal state
        /// </summary>
        private readonly IntPtr zfree;

        /// <summary>
        /// private data object passed to zalloc and zfree
        /// </summary>
        private readonly IntPtr opaque;

        /// <summary>
        /// best guess about the data type: binary or text
        /// for deflate, or the decoding state for inflate
        /// </summary>
        public ZLibDataType data_type;

        /// <summary>
        /// Adler-32 or CRC-32 value of the uncompressed data
        /// </summary>
        public uint adler;

        /// <summary>
        /// reserved for future use
        /// </summary>
        private readonly uint reserved;
    }
#pragma warning restore IDE1006 // Naming Styles

    public enum ZLibReturnCode
    {
        Ok = 0,
        StreamEnd = 1,
        NeedDictionary = 2,
        ErrNo = -1,
        StreamError = -2,
        DataError = -3,
        MemoryError = -4,
        BufferError = -5,
        VersionError = -6,
    }

    internal enum ZLibFlush
    {
        NoFlush = 0,
        PartialFlush = 1,
        SyncFlush = 2,
        FullFlush = 3,
        Finish = 4,
    }

    internal enum ZLibCompressionStrategy
    {
        Default = 0,
        Filtered = 1,
        HuffmanOnly = 2,
    }

    internal enum ZLibMemLevel
    {
        Default = 8,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
    }

    internal enum ZLibWindowBits
    {
        Default = 15,
        Bits9 = 9,
        Bits10 = 10,
        Bits11 = 11,
        Bits12 = 12,
        Bits13 = 13,
        Bits14 = 14,
        Bits15 = 15,
    }

    public enum CompressionMode
    {
        Compress,
        Decompress,
    }

    public enum CompressionLevel
    {
        Default = -1,
        NoCompression = 0,
        BestSpeed = 1,
        BestCompression = 9,
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
    }

    internal enum ZLibDataType
    {
        Binary = 0,
        Ascii = 1,
        Unknown = 2,
    }

    internal enum ZLibCompressionMethod
    {
        Deflated = 8,
    }
}
