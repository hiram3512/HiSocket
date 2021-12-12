/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HiSocket.Tcp.Example
{
    public class ExamplePackage1 : IPackage
    {
        public void Pack(byte[] message, IBlockBuffer<byte> sendBuffer)
        {
            sendBuffer.WriteAtEnd(message);
        }
        public void Unpack(IBlockBuffer<byte> receiveBuffer, ref byte[] message)
        {
            var length = receiveBuffer.Index;
            if (length > 0)
            {
                message = receiveBuffer.ReadFromHead(length);
            }
        }
    }
}