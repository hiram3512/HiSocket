using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    class BlockBuffer<T> : IBlockBuffer<T>
    {
        public T[] Buffer { get; }
        public int TotalCapcity { get; }
        public int Index { get; private set; }

        public BlockBuffer(int capcity)
        {
            if (capcity <= 0)
            {
                throw new ArgumentException("capcity<=0");
            }
            Index = 0;
            Buffer = new T[capcity];
        }

        public T[] Read(int count) { throw new NotImplementedException(); }

        public void WriteHead(T[] data, int sourceIndex, int sourceLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data is null");
            }
            if (data.Length <= 0)
            {
                throw new ArgumentNullException("data.Length<=0");
            }
            if (sourceIndex < 0 || sourceIndex > data.Length)
            {
                throw new ArgumentNullException("sourceIndex error");
            }
            if (sourceLength < 0 || sourceLength > data.Length - sourceIndex)
            {
                throw new ArgumentNullException("sourceLength error");
            }
            int currentLength = Index + sourceLength;
            if (currentLength >= TotalCapcity)
            {
                throw new Exception("buffer full, make a larger buffer");
            }
            //for avoid over write data, first move second segment
            if (Index > 0)
            {
                Array.Copy(Buffer, 0, Buffer, sourceLength, Index);
            }
            Array.Copy(data, sourceIndex, Buffer, 0, sourceLength);
            IncreaseIndex(sourceLength);
        }

        public void WriteEnd(T[] data, int sourceIndex, int sourceLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data is null");
            }
            if (data.Length <= 0)
            {
                throw new ArgumentNullException("data.Length<=0");
            }
            if (sourceIndex < 0 || sourceIndex > data.Length)
            {
                throw new ArgumentNullException("sourceIndex error");
            }
            if (sourceLength < 0 || sourceLength > data.Length - sourceIndex)
            {
                throw new ArgumentNullException("sourceLength error");
            }
            int currentLength = Index + sourceLength;
            if (currentLength >= TotalCapcity)
            {
                throw new Exception("buffer full, make a larger buffer");
            }
            Array.Copy(data, sourceIndex, Buffer, Index, sourceLength);
            IncreaseIndex(sourceLength);
        }

        public int GetCurrentCapcity()
        {
            int remain = TotalCapcity - Index;
            if (remain < 0)
            {
                throw new Exception("remain<0");
            }
            return remain;
        }

        public void IncreaseIndex(int length)
        {
            Index += length;
            if (Index >= TotalCapcity)
            {
                throw new Exception("buffer full, make a larger buffer");
            }
        }
        public void DecreaseIndex(int length)
        {
            Index -= length;
            if (Index < 0)
            {
                throw new Exception("Index < 0");
            }
        }

        public void RemoveFront(int length)
        {
            int currentIndex = Index - length;
            if (currentIndex < 0)
            {
                throw new Exception("no so many data to remove");
            }
            Array.Copy(Buffer, Index, Buffer, 0, length);
            DecreaseIndex(length);
        }
    }
}
