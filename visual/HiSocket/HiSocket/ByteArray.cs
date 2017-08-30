//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

using System;
using System.Collections.Generic;

namespace HiSocket
{
    internal class ByteArray : IByteArray
    {
        List<byte> bytes = new List<byte>();
        public int Length
        {
            get { return bytes.Count; }
        }

        public byte[] Read(int length)
        {
            if (length > this.bytes.Count)
            {
                throw new Exception("length>bytes's length");
            }
            lock (this.bytes)
            {
                byte[] bytes = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    bytes[i] = this.bytes[i];
                }
                this.bytes.RemoveRange(0, length);
                return bytes;
            }
        }

        public void Write(byte[] bytes, int length)
        {
            if (length > bytes.Length)
            {
                throw new Exception("length>bytes's length");
            }
            lock (this.bytes)
            {
                for (int i = 0; i < length; i++)
                {
                    this.bytes.Add(bytes[i]);
                }
            }
        }

        public void Clear()
        {
            lock (bytes)
            {
                bytes.Clear();
            }
        }
    }
}


