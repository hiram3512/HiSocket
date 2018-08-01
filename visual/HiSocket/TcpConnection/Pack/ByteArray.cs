/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class ByteArray : IByteArray
    {
        private readonly List<byte> bytes = new List<byte>();
        private readonly object locker = new object();
        public int Length
        {
            get
            {
                lock (locker)
                {
                    return bytes.Count;
                }
            }
        }

        public byte[] Read(int length)
        {
            lock (locker)
            {
                if (length > Length)
                    throw new Exception("length error: don't have so many bytes to read");
                var bytes = this.bytes.GetRange(0, length);
                this.bytes.RemoveRange(0, length);
                return bytes.ToArray();
            }
        }

        public void Write(byte[] bytes)
        {
            lock (locker)
            {
                this.bytes.InsertRange(this.bytes.Count, bytes);
            }
        }

        public void Insert(int index, byte[] bytes)
        {
            lock (locker)
            {
                this.bytes.InsertRange(index, bytes);
            }
        }
    }
}