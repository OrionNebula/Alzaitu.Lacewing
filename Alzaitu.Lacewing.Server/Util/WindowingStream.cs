using System;
using System.IO;

namespace Alzaitu.Lacewing.Server.Util
{
    internal class WindowingStream : Stream
    {
        public long ByteLimit { get; }

        public long LimitProgress { get; private set; }

        public Stream BaseStream { get; }

        public WindowingStream(Stream baseStream, long byteLimit)
        {
            BaseStream = baseStream;
            ByteLimit = byteLimit;
        }

        public override void Flush() => BaseStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (ByteLimit - LimitProgress <= 0)
                return -1;

            var readProg = BaseStream.Read(buffer, offset, (int)Math.Min(ByteLimit - LimitProgress, count));

            LimitProgress += readProg;

            return readProg;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException();
        }

        public override void SetLength(long value)
        {
            throw new InvalidOperationException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if(ByteLimit - LimitProgress <= 0)
                throw new EndOfStreamException();

            var wrtLen = (int)Math.Min(ByteLimit - LimitProgress, count);
            BaseStream.Write(buffer, offset, wrtLen);
            LimitProgress += wrtLen;
        }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => false;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => ByteLimit;

        public override long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }
    }
}
