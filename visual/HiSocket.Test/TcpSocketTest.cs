using HiSocket.Tcp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiSocket.Test
{
    [TestClass]
    public class TcpSocketTest
    {
        private TcpServer _server;
        private ITcpSocket _tcp;
        [TestInitialize]
        public void Init()
        {
            _server = new TcpServer();
        }
        [TestCleanup]
        public void Cleanup()
        {
            _server.Close();
            _server = null;
        }

        [TestMethod]
        public void TestEvent()
        {
            _tcp = new TcpSocket();
            bool isConnecting = false;
            _tcp.OnConnecting += () => { isConnecting = true; };
            bool isConnected = false;
            _tcp.OnConnected += () => { isConnected = true; };
            bool isDisconnected = false;
            _tcp.OnDisconnected += () => { isDisconnected = true; };
            _tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnect(_tcp);
            _tcp.DisConnect();
            Assert.IsTrue(isConnecting);
            Assert.IsTrue(isConnected);
            Assert.IsTrue(isDisconnected);
        }

        [TestMethod]
        public void TestSendReceive()
        {
            _tcp = new TcpSocket();
            _tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnect(_tcp);
            int length = 0;
            _tcp.OnSocketReceive += (x) =>
            {
                length = x.Length;
            };
            _tcp.Send(new byte[10]);
            Common.WaitValue(ref length, 10);
            _tcp.DisConnect();
            Assert.AreEqual(length, 10);
        }
    }
}
