using System;
using System.Collections;
using System.IO;

namespace HiSocket
{
    public class MsgHandler
    {
        private Queue sendQueue;
        private Queue receiveQueue;
        private MemoryStream stream;
        private BinaryReader reader;
        private TCP.ClientTcp client;
        private long remainingBytes
        {
            get { return stream.Length - stream.Position; }
        }
        public MsgHandler(TCP.ClientTcp param)
        {
            client = param;
            sendQueue = new Queue();
            receiveQueue = new Queue();
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
            MsgManager.Instance.Init(this);
        }
        public void Receive(byte[] paramBytes, int paramLength)
        {
            stream.Seek(0, SeekOrigin.End);
            stream.Write(paramBytes, 0, paramLength);
            stream.Seek(0, SeekOrigin.Begin);
            while (remainingBytes > MsgDefine.length)
            {
                UInt16 msgLength = reader.ReadUInt16();
                msgLength += MsgDefine.flag;
                if (remainingBytes >= msgLength)
                {
                    stream.Position += MsgDefine.flag;
                    msgLength -= MsgDefine.flag;
                    byte[] bytes = reader.ReadBytes(msgLength);
                    receiveQueue.Enqueue(bytes);
                }
                else
                {
                    stream.Position -= MsgDefine.length;
                    break;
                }
            }
            byte[] leftover = reader.ReadBytes((int)remainingBytes);
            stream.SetLength(0);
            stream.Write(leftover, 0, leftover.Length);
            Receive();
        }
        void Receive()
        {
            lock (this)
            {
                while (receiveQueue.Count > 0)
                {
                    MsgManager.Instance.ReceiveMsg((byte[])receiveQueue.Dequeue());
                }
            }
        }
        public void Send(byte[] paramBytes)
        {
            sendQueue.Enqueue(paramBytes);
            lock (this)
            {
                while (sendQueue.Count > 0)
                {
                    client.Send((byte[])sendQueue.Dequeue());
                }
            }
        }
        public void Close()
        {
            client = null;
            reader.Close();
            stream.Close();
            sendQueue.Clear();
            receiveQueue.Clear();
        }
    }
}