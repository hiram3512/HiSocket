using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    public abstract class PackageBase:IPackage
    {
        public void Pack(byte[] data, IBlockBuffer<byte> buffer)
        {
            buffer.WriteHead(data, 0, data.Length);
            buffer.WriteHead(new byte[2],0,2);
        }

        public void Unpack(IBlockBuffer<byte> buffer, byte[] data)
        {
            buffer.Read(2);
        }
    }
}