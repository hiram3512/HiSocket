/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{

    public class BlockBuffer<T> : IBlockBuffer<T>
    {
        public T[] Buffer { get; private set; }
        public int TotalCapcity { get; }
        public int Index { get; private set; }
        public BlockBuffer(int capcity)
        {
            if (capcity <= 0)
            {
                throw new Exception("capcity<=0");
            }
            Index = 0;
            TotalCapcity = capcity;
            Buffer = new T[capcity];
        }

        public T[] ReadFromHead(int count)
        {
            var data = TryReadFromHead(count);
            DecreaseIndex(count);
            return data;
        }

        public T[] TryReadFromHead(int count)
        {
            if (count <= 0)
            {
                throw new Exception("count<=0");
            }
            if (count > Index)
            {
                throw new Exception("have no so many data");
            }
            T[] data = new T[count];
            Array.Copy(Buffer, 0, data, 0, count);
            int remain = Index - count;
            if (remain > 0)
            {
                Array.Copy(Buffer, count, Buffer, 0, remain);
            }
            return data;
        }

        public void WriteAtEnd(T[] data, int sourceIndex, int sourceLength)
        {
            if (data == null || data.Length <= 0)
            {
                throw new Exception("data null or empty");
            }
            if (sourceIndex < 0 || sourceIndex >= data.Length)
            {
                throw new Exception("sourceIndex error");
            }
            if (sourceLength < 0 || sourceLength > data.Length - sourceIndex)
            {
                throw new Exception("sourceLength error");
            }
            int finalLength = Index + sourceLength;
            if (finalLength > TotalCapcity)
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
            if (Index > TotalCapcity)
            {
                throw new Exception("buffer full, make a larger buffer");
            }
        }
        private void DecreaseIndex(int length)
        {
            Index -= length;
            if (Index < 0)
            {
                throw new Exception("Index < 0");
            }
        }

        public void WriteAtEnd(T[] data)
        {
            if (data == null || data.Length <= 0)
            {
                throw new Exception("data null or empty");
            }
            WriteAtEnd(data, 0, data.Length);
        }

        public void Dispose()
        {
            Buffer = null;
        }
    }
}
