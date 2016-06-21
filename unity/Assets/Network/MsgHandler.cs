//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System.Collections;
using System.IO;

namespace HiSocket
{
    internal class MsgHandler
    {
        private Queue sendQueue;
        private Queue receiveQueue;
        private MemoryStream ms;
        private BinaryReader br;

        private long remainingBytesSize { get { return ms.Length - ms.Position; } }

        public void Receive(byte[] paramBytes, int paramLength)
        {
            ms.Seek(0, SeekOrigin.End);
            ms.Write(paramBytes, 0, paramLength);
            ms.Seek(0, SeekOrigin.Begin);

            while (remainingBytesSize>10)
            {
                
            }
        }

    }
}