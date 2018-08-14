using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace HiSocket.Test
{
    [TestClass]
    public class TcpSocketTest
    {
        private TcpServer server;

        /// <summary>
        /// Test socket connect 
        /// </summary>
        [TestMethod]
        public void TestConnect()
        {
            TcpSocket tcp = new TcpSocket();
            tcp.Connect(Common.GetIpEndPoint());

            //Test connected
            Common.WaitConnected(tcp);
            Assert.IsTrue(tcp.IsConnected);

            //Test disconnect
            tcp.DisConnect();
            Assert.IsFalse(tcp.IsConnected);

            tcp.Dispose();
        }

        /// <summary>
        /// Test diffrent connect api
        /// </summary>
        [TestMethod]
        public void TestConnectAPI()
        {
            TcpSocket tcp = new TcpSocket();
            tcp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7777));
            Common.WaitConnected(tcp);
            Assert.IsTrue(tcp.IsConnected);
            tcp.Dispose();

            tcp = new TcpSocket();
            tcp.Connect(IPAddress.Parse("127.0.0.1"), 7777);
            Common.WaitConnected(tcp);
            Assert.IsTrue(tcp.IsConnected);
            tcp.Dispose();

            tcp = new TcpSocket();
            tcp.Connect("127.0.0.1",7777);
            Common.WaitConnected(tcp);
            Assert.IsTrue(tcp.IsConnected);
            tcp.Dispose();
        }

        /// <summary>
        /// Test event trigger
        /// </summary>
        [TestMethod]
        public void TestEvent()
        {
            bool isOnConnecting = false;
            bool isOnConnected = false;
            bool isOnDisconnected = false;
            bool isOnSend = false;
            bool isOnReceive = false;

            TcpSocket tcp = new TcpSocket();
            tcp.OnConnecting += () => { isOnConnecting = true; };
            tcp.OnConnected += () => { isOnConnected = true; };
            tcp.OnDisconnected += () => { isOnDisconnected = true; };
            tcp.OnSocketSend += (x) => { isOnSend = true; };
            tcp.OnSocketReceive += (x) => { isOnReceive = true; };

            tcp.Connect(Common.GetIpEndPoint());
            Assert.IsTrue(isOnConnecting);

            Common.WaitTrue(ref isOnConnected);
            Assert.IsTrue(isOnConnected);

            tcp.Send(new byte[10]);
            Common.WaitTrue(ref isOnSend);
            Assert.IsTrue(isOnSend);

            Common.WaitTrue(ref isOnReceive);
            Assert.IsTrue(isOnReceive);

            tcp.DisConnect();
            Assert.IsTrue(isOnDisconnected);

        }

        /// <summary>
        /// Test data send and receive with server
        /// </summary>
        [TestMethod]
        public void TestSendReceive()
        {
            var tcp = new TcpSocket();
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnected(tcp);
            int length = 0;
            tcp.OnSocketReceive += (x) =>
            {
                length = x.Length;
            };
            tcp.Send(new byte[10]);
            Common.WaitValue(ref length, 10);
            tcp.DisConnect();
            Assert.AreEqual(length, 10);
        }

        /// <summary>
        /// Test large data
        /// </summary>
        [TestMethod]
        public void TestLargeMessage()
        {
            var tcp = new TcpSocket();
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnected(tcp);
            int length = 0;
            tcp.OnSocketReceive += (x) =>
            {
                length += x.Length;
            };
            tcp.Send(new byte[1 << 10]);
            Common.WaitValue(ref length, 1 << 10, 10000);
            tcp.DisConnect();
            Console.WriteLine(length);
            Assert.AreEqual(length, 1 << 10);
        }

        [TestInitialize]
        public void Init()
        {
            server = new TcpServer();
        }

        [TestCleanup]
        public void Cleanup()
        {
            server.Close();
            server = null;
        }
    }
}
