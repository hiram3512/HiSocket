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
            get { return _bytes.Count; }
        }

        public byte[] Read(int length)
        {
            lock (_locker)
            {
                try
                {
                    byte[] bytes = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        bytes[i] = this._bytes[i];
                    }
                    this._bytes.RemoveRange(0, length);
                    return bytes;
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }

            }
        }

        public void Write(byte[] bytes, int length)
        {
            lock (_locker)
            {
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        this._bytes.Add(bytes[i]);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        public void Insert(int index, byte[] bytes)
        {
            lock (_locker)
            {
                _bytes.InsertRange(index, bytes);
            }
        }

        public byte[] ToArray()
        {
            lock (_locker)
            {
                return _bytes.ToArray();
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