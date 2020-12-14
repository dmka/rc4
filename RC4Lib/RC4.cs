using System;
using System.Security.Cryptography;

namespace RC4Lib
{
    public class RC4 : ICryptoTransform
    {
        private byte[] _state;
        private byte _i;
        private byte _j;

        public RC4(byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (key.Length == 0)
                throw new ArgumentException("Invalid length", "key");

            _i = 0;
            _j = 0;
            _state = new byte[256];

            SetKey(key);
        }


        public bool CanReuseTransform => false;

        public bool CanTransformMultipleBlocks => true;

        public int InputBlockSize => 1;

        public int OutputBlockSize => 1;

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            ValidateInputArgs(inputBuffer, inputOffset, inputCount);
            ValidateOutputArgs(outputBuffer, outputOffset, inputCount);

            InternalEncrypt(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            ValidateInputArgs(inputBuffer, inputOffset, inputCount);

            byte[] outputBuffer = new byte[inputCount];
            InternalEncrypt(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        public void Dispose()
        {

        }

        private void SetKey(byte[] key)
        {
            for (int i = 0; i < _state.Length; i++)
            {
                _state[i] = (byte)i;
            }

            for (int i = 0, j = 0; i < _state.Length; i++)
            {
                j = (byte)(j + _state[i] + key[i % key.Length]);
                byte t = _state[i];
                _state[i] = _state[j];
                _state[j] = t;
            }
        }

        private void InternalEncrypt(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            while (inputCount-- > 0)
            {
                _i = (byte)(_i + 1);
                _j = (byte)(_j + _state[_i]);
                byte t = _state[_i];
                _state[_i] = _state[_j];
                _state[_j] = t;
                byte r = _state[(byte)(_state[_i] + _state[_j])];
                outputBuffer[outputOffset++] = (byte)(inputBuffer[inputOffset++] ^ r);
            }
        }

        private static void ValidateInputArgs(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
                throw new ArgumentNullException("inputBuffer");

            if (inputCount < 0)
                throw new ArgumentOutOfRangeException("inputCount");

            if (inputOffset < 0 || inputOffset > inputBuffer.Length - inputCount)
                throw new ArgumentOutOfRangeException("inputOffset");
        }

        private static void ValidateOutputArgs(byte[] outputBuffer, int outputOffset, int outputCount)
        {
            if (outputBuffer == null)
                throw new ArgumentNullException("outputBuffer");

            if (outputOffset > outputBuffer.Length - outputCount)
                throw new ArgumentOutOfRangeException("outputOffset");
        }
    }
}
