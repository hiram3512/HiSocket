//*********************************************************************
// Description:
// Author: hiramtan@live.com
//*********************************************************************

using System;
using System.Collections;
using System.IO;
using HiSocket.TCP;

namespace HiSocket
{
    internal class MsgHandler
    {
        private Queue sendQueue;
        private Queue receiveQueue;
        private MemoryStream ms;
        private BinaryReader br;
        private long remainingBytesSize { get { return ms.Length - ms.Position; } }
        private ISocket iSocket;


        public MsgHandler(ISocket param)
        {
            iSocket = param;
            sendQueue = new Queue();
            receiveQueue = new Queue();
            ms = new MemoryStream();
            br = new BinaryReader(ms);
            MsgManager.Init(param);
        }

        public void Send(byte[] param)
        {
            sendQueue.Enqueue(param);
            lock (this)
            {
                while (sendQueue.Count > 0)
                {
                    iSocket.Send((byte[])sendQueue.Dequeue());
                }
            }
        }

        public void Receive(byte[] paramBytes, int paramLength)
        {
            ms.Seek(0, SeekOrigin.End);
            ms.Write(paramBytes, 0, paramLength);
            ms.Seek(0, SeekOrigin.Begin);
            while (remainingBytesSize > sizeof(uint))
            {
                UInt16 tempMsgLength = br.ReadUInt16();
                ms.Position -= sizeof(uint);
                if (remainingBytesSize >= tempMsgLength)
                {
                    byte[] tempBytes = br.ReadBytes(tempMsgLength);
                    receiveQueue.Enqueue(tempBytes);
                }
                else
                    break;
            }
            byte[] tempLeftBytes = br.ReadBytes((int)remainingBytesSize);
            ms.SetLength(0);
            ms.Write(tempLeftBytes, 0, tempLeftBytes.Length);
            Receive();
        }

        private void Receive()
        {
            lock (this)
            {
                while (receiveQueue.Count > 0)
                {
                    MsgManager.ReceiveMsg((byte[])receiveQueue.Dequeue());
                }
            }
        }
    }
}