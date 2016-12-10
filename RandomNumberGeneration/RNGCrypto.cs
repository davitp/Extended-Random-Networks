using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace RandomNumberGeneration
{
    // Реализация генератора криптографически сильных случайных чисел.
    public class RNGCrypto
    {
        private const int BufferSize = 1024;  // must be a multiple of 4
        private byte[] RandomBuffer;
        private int BufferOffset;
        private RNGCryptoServiceProvider rng;

        public RNGCrypto()
        {
            RandomBuffer = new byte[BufferSize];
            rng = new RNGCryptoServiceProvider();
            BufferOffset = RandomBuffer.Length;
        }

        // Возвращает случайное положительное целое число.
        public int Next()
        {
            if (BufferOffset >= RandomBuffer.Length)
            {
                FillBuffer();
            }

            int val = BitConverter.ToInt32(RandomBuffer, BufferOffset) & 0x7fffffff;
            BufferOffset += sizeof(int);
            return val;
        }

        // Возвращает случайное положительное целое число (меньше данного значения).
        public int Next(int maxValue)
        {
            return Next() % maxValue;
        }

        // // Возвращает случайное положительное целое число из данного диапазона.
        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("maxValue must be greater than or equal to minValue");
            }
            int range = maxValue - minValue;
            return minValue + Next(range);
        }

        // Возвращает случайное положительное дейвствительное число.
        public double NextDouble()
        {
            int val = Next();
            return (double)val / int.MaxValue;
        }

        // Возвращает случайное положительное дейвствительное число из данного диапазона.
        public double NextDouble(double a, double b)
        {
            return a + (b - a) / NextDouble();
        }

        public void GetBytes(byte[] buff)
        {
            rng.GetBytes(buff);
        }

        /*public double NextDouble()
        {
            byte[] data = new byte[4];
            rng.GetBytes(data);
            return (double)BitConverter.ToUInt32(data, 0) / UInt32.MaxValue;
        }

        public double NextDouble(double a, double b)
        {
            byte[] data = new byte[8];
            rng.GetBytes(data);
            return a + (b - a) / BitConverter.ToDouble(data, 0);
        }*/

        // Utilities

        private void FillBuffer()
        {
            rng.GetBytes(RandomBuffer);
            BufferOffset = 0;
        }
    }
}
