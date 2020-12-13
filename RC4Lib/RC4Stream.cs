using System;
using System.IO;

namespace RC4Lib
{
    public class RC4Stream : Stream
    {
        private RC4 _rc4;
        private Stream _inputStream;
        private byte[] _inputBuffer;
        private int _inputBufferCount;
        private int _inputOffsetBuffer;

        public RC4Stream(byte[] key, Stream inputStream)
        {
            if (inputStream == null)
                throw new ArgumentNullException("inputStream");

            if (!inputStream.CanRead)
                throw new ArgumentException("Not readable stream", "sourceStream");

            _inputStream = inputStream;
            _rc4 = new RC4(key);
            _inputBuffer = new byte[1024 * 4];
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if (offset > buffer.Length - count)
                throw new ArgumentOutOfRangeException("offset");

            int outputOffset = offset;
            int outputRemainingCount = count;

            while (true)
            {
                var inputCount = Math.Min(_inputBufferCount, outputRemainingCount);

                _rc4.TransformBlock(_inputBuffer, _inputOffsetBuffer, inputCount, buffer, outputOffset);

                _inputOffsetBuffer += inputCount;
                _inputBufferCount -= inputCount;

                outputOffset += inputCount;
                outputRemainingCount -= inputCount;

                if (outputRemainingCount == 0)
                {
                    break;
                }

                int bytesRead = _inputStream.Read(_inputBuffer, 0, _inputBuffer.Length);

                if (bytesRead == 0)
                {
                    break;
                }

                _inputBufferCount = bytesRead;
                _inputOffsetBuffer = 0;
            }

            return count - outputRemainingCount;
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}