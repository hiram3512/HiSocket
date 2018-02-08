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
                var bytes = _bytes.GetRange(index, length);
                _bytes.RemoveRange(index, length);
                return bytes.ToArray();
            }
        }

        public byte[] Read(int length)
        {
            lock (_locker)
            {
                return Read(0, length);
            }
        }

        public void Write(int index, byte[] bytes)
        {
            lock (_locker)
            {
                _bytes.InsertRange(index, bytes);
            }
        }

        public void Write(byte[] bytes, int length)
        {
            lock (_locker)
            {
                for (int i = 0; i < length; i++)
                {
                    _bytes.Add(bytes[i]);
                }
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