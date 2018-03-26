//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************

using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class ByteArray : IByteArray
    {
        private readonly List<byte> _bytes = new List<byte>();
        private readonly object _locker = new object();
        public int Length
        {
            get
            {
                lock (_locker)
                {
                    return _bytes.Count;
                }
            }
        }

        public byte[] Read(int index, int length)
        {
            lock (_locker)
            {
                if (index + length > Length)
                    throw new Exception("length error: don't have so many bytes to read");
                var bytes = _bytes.GetRange(index, length);
                _bytes.RemoveRange(index, length);
                return bytes.ToArray();
            }
        }

        public void Write(int insertIndex, byte[] bytes)
        {
            lock (_locker)
            {
                _bytes.InsertRange(insertIndex, bytes);
            }
        }
        public void Clear()
        {
            lock (_locker)
            {
                _bytes.Clear();
            }
        }
    }
}