using System;
using System.IO;

namespace SatisfactorySaveParser.ZLib
{
    public class DeflateStream : Stream
    {
        protected enum Format
        {
            Deflate,
            ZLib,
            GZip,
            BothZLibGzip, // Valid only in Decompress mode
        }

        private readonly CompressionMode compressionMode;
        private z_streamp strm;
        private bool disposed;

        readonly byte[] workBuffer = new byte[0x1000];
        int workBufferPos;

        protected virtual Format OpenType => Format.Deflate;
        protected virtual Format WriteType => Format.Deflate;

        public Stream BaseStream { get; private set; }
        public long TotalIn { get; private set; }
        public long TotalOut { get; private set; }

        public override bool CanRead => compressionMode == CompressionMode.Decompress && BaseStream.CanRead;
        public override bool CanWrite => compressionMode == CompressionMode.Compress && BaseStream.CanWrite;
        public override bool CanSeek => false;

        public override long Length => throw new NotSupportedException($"{nameof(Length)} is not supported");
        public override long Position
        {
            get => throw new NotSupportedException($"{nameof(Position)} is not supported");
            set => throw new NotSupportedException($"{nameof(Position)} is not supported");
        }

        public DeflateStream(Stream baseStream, CompressionMode mode)
            : this(baseStream, mode, CompressionLevel.Default)
        {
        }

        public DeflateStream(Stream baseStream, CompressionMode mode, CompressionLevel compressionLevel)
        {
            BaseStream = baseStream;
            compressionMode = mode;

            ZLibReturnCode ret;
            if (mode == CompressionMode.Compress)
                ret = NativeMethods.DeflateInit(ref strm, compressionLevel, GetWindowBits(ZLibWindowBits.Default, mode, WriteType));
            else
                ret = NativeMethods.InflateInit(ref strm, GetWindowBits(ZLibWindowBits.Default, mode, OpenType));

            if (ret != ZLibReturnCode.Ok)
                throw new ZLibException(ret, strm.LastErrorMessage);
        }

        ~DeflateStream()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !disposed)
            {
                if (BaseStream != null)
                {
                    if (compressionMode == CompressionMode.Compress)
                        Flush();

                    BaseStream = null;
                }

                if (compressionMode == CompressionMode.Compress)
                    NativeMethods.DeflateEnd(ref strm);
                else
                    NativeMethods.InflateEnd(ref strm);

                disposed = true;
            }

            base.Dispose(disposing);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException($"{nameof(Seek)} is not supported");
        public override void SetLength(long value) => throw new NotSupportedException($"{nameof(SetLength)} is not supported");

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (compressionMode != CompressionMode.Decompress)
                throw new NotSupportedException("Can only read from a decompression stream");

            if (count == 0)
                return 0;

            return Read(buffer.AsSpan(offset, count));
        }

        public unsafe int Read(Span<byte> buffer)
        {
            if (compressionMode == CompressionMode.Compress)
                throw new NotSupportedException("Can't read from a compression stream");

            if (workBufferPos == -1)
                return 0;

            int readLen = 0;
            fixed (byte* readPtr = workBuffer)
            fixed (byte* writePtr = buffer)
            {
                strm.next_in = (IntPtr)(readPtr + workBufferPos);
                strm.next_out = (IntPtr)writePtr;
                strm.avail_out = (uint)buffer.Length;

                while (strm.avail_out != 0)
                {
                    if (strm.avail_in == 0)
                    {
                        workBufferPos = 0;
                        strm.next_in = (IntPtr)readPtr;
                        strm.avail_in = (uint)BaseStream.Read(workBuffer, 0, workBuffer.Length);
                        TotalIn += strm.avail_in;
                    }

                    uint inCount = strm.avail_in;
                    uint outCount = strm.avail_out;

                    var ret = NativeMethods.Inflate(ref strm, ZLibFlush.NoFlush); // flush method for inflate has no effect

                    workBufferPos += (int)(inCount - strm.avail_in);
                    readLen += (int)(outCount - strm.avail_out);

                    if (ret == ZLibReturnCode.StreamEnd)
                    {
                        workBufferPos = -1;
                        break;
                    }

                    if (ret != ZLibReturnCode.Ok)
                        throw new ZLibException(ret, strm.LastErrorMessage);
                }
            }

            TotalOut += readLen;
            return readLen;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (compressionMode != CompressionMode.Compress)
                throw new NotSupportedException("Can only write to a compression stream");

            if (count == 0)
                return;

            Write(buffer.AsSpan(offset, count));
        }

        public unsafe void Write(ReadOnlySpan<byte> buffer)
        {
            if (compressionMode != CompressionMode.Compress)
                throw new NotSupportedException("Can only write to a compression stream");

            TotalIn += buffer.Length;

            fixed (byte* readPtr = buffer)
            fixed (byte* writePtr = workBuffer)
            {
                strm.next_in = (IntPtr)readPtr;
                strm.avail_in = (uint)buffer.Length;
                strm.next_out = (IntPtr)(writePtr + workBufferPos);
                strm.avail_out = (uint)(workBuffer.Length - workBufferPos);

                while (strm.avail_in != 0)
                {
                    uint outCount = strm.avail_out;
                    var ret = NativeMethods.Deflate(ref strm, ZLibFlush.NoFlush);
                    workBufferPos += (int)(outCount - strm.avail_out);

                    if (strm.avail_out == 0)
                    {
                        BaseStream.Write(workBuffer, 0, workBuffer.Length);
                        TotalOut += workBuffer.Length;

                        workBufferPos = 0;
                        strm.next_out = (IntPtr)writePtr;
                        strm.avail_out = (uint)workBuffer.Length;
                    }

                    if (ret != ZLibReturnCode.Ok)
                        throw new ZLibException(ret, strm.LastErrorMessage);
                }
            }
        }

        public override unsafe void Flush()
        {
            if (compressionMode == CompressionMode.Decompress)
            {
                BaseStream.Flush();
                return;
            }

            fixed (byte* writePtr = workBuffer)
            {
                strm.next_in = IntPtr.Zero;
                strm.avail_in = 0;
                strm.next_out = (IntPtr)(writePtr + workBufferPos);
                strm.avail_out = (uint)(workBuffer.Length - workBufferPos);

                var ret = ZLibReturnCode.Ok;
                while (ret != ZLibReturnCode.StreamEnd)
                {
                    if (strm.avail_out != 0)
                    {
                        uint outCount = strm.avail_out;
                        ret = NativeMethods.Deflate(ref strm, ZLibFlush.Finish);

                        workBufferPos += (int)(outCount - strm.avail_out);

                        if (ret != ZLibReturnCode.Ok && ret != ZLibReturnCode.StreamEnd)
                            throw new ZLibException(ret, strm.LastErrorMessage);
                    }

                    BaseStream.Write(workBuffer, 0, workBufferPos);
                    TotalOut += workBufferPos;

                    workBufferPos = 0;
                    strm.next_out = (IntPtr)writePtr;
                    strm.avail_out = (uint)workBuffer.Length;
                }
            }

            BaseStream.Flush();
        }

        private static int GetWindowBits(ZLibWindowBits windowBits, CompressionMode mode, Format format)
        {
            int bits = (int)windowBits;
            switch (format)
            {
                case Format.Deflate:
                    return bits * -1;
                case Format.GZip:
                    return bits += 16;
                case Format.ZLib:
                    return bits;
                case Format.BothZLibGzip:
                    if (mode == CompressionMode.Decompress)
                        return bits += 32;
                    else
                        throw new ArgumentException("Only support for decompressing", nameof(format));
                default:
                    throw new ArgumentOutOfRangeException(nameof(format), "Not a valid format");
            }
        }
    }
}
