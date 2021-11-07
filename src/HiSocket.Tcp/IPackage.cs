using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    interface IPackage
    {
        void Pack(byte[] data, IBlockBuffer<byte> buffer);
        void Unpack(IBlockBuffer<byte> buffer, byte[] data);
    }
}