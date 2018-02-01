using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HiSocket;

namespace NUnit.Tests
{
    [TestFixture]
    public class TestTcp
    {
        [Test]
        public void TestMethod()
        {
            // TODO: Add your test code here
            Assert.Pass("Your first passing test");
        }
        Package package = new Package();
        TcpConnection tcp;
        void Start()
        {
            tcp = new TcpConnection(package);
            tcp.Connect("127.0.0.1", 7777);

            tcp.StateChangeEvent += OnStateChange;
            tcp.ReceiveEvent += OnReceive;

            tcp.Send(new byte[1]);

            tcp.DisConnect();
        }

        void Update()
        {
            tcp.Run();
        }

        void OnReceive(byte[] bytes)
        {

        }

        void OnStateChange(SocketState state)
        {

        }

        void OnApplicationQuit()
        {
            tcp.DisConnect();
        }
    }

    public class Package : IPackage
    {
        public void Unpack(IByteArray reader, Queue<byte[]> receiveQueue)
        {
            //get head length or id
            while (reader.Length >= 1)
            {
                byte bodyLength = reader.Read(1)[0];

                if (reader.Length >= bodyLength)
                {
                    var body = reader.Read(bodyLength);
                    receiveQueue.Enqueue(body);
                }
            }
        }

        public void Pack(Queue<byte[]> sendQueue, IByteArray writer)
        {
            //add head length or id
            byte[] head = new Byte[1] { 4 };
            writer.Write(head, head.Length);

            var body = sendQueue.Dequeue();
            writer.Write(body, body.Length);
        }
    }
}
