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

            tcp.StateChangeHandler = OnStateChange;
            tcp.ReceiveHandler = OnReceive;

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
    }

    public class Package : IPackage
    {
        public void Unpack(IByteArray reader, out byte[] writer)
        {
            //拆包逻辑
            throw new NotImplementedException();
        }

        public void Pack(ref byte[] reader, IByteArray writer)
        {
            //封包逻辑
            throw new NotImplementedException();
        }
    }
}
