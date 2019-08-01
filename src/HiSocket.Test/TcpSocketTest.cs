using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using HiSocket.Tcp;

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
            tcp.Disconnect();
            Assert.IsFalse(tcp.IsConnected);
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
            tcp.OnConnecting += (x) => { isOnConnecting = true; };
            tcp.OnConnected += (x) => { isOnConnected = true; };
            tcp.OnDisconnected += (x) => { isOnDisconnected = true; };
            tcp.OnSendBytes += (x,y) => { isOnSend = true; };
            tcp.OnReceiveBytes += (x,y) => { isOnReceive = true; };

            tcp.Connect(Common.GetIpEndPoint());
            Assert.IsTrue(isOnConnecting);

            Common.WaitTrue(ref isOnConnected);
            Assert.IsTrue(isOnConnected);

            tcp.Send(new byte[10]);
            Common.WaitTrue(ref isOnSend);
            Assert.IsTrue(isOnSend);

            Common.WaitTrue(ref isOnReceive);
            Assert.IsTrue(isOnReceive);

            tcp.Disconnect();
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
            tcp.OnReceiveBytes += (x,y) =>
            {
                length = y.Length;
            };
            tcp.Send(new byte[10]);
            Common.WaitValue(ref length, 10);
            tcp.Disconnect();
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
            tcp.OnReceiveBytes += (x,y) =>
            {
                length += y.Length;
            };
            tcp.Send(new byte[1 << 10]);
            Common.WaitValue(ref length, 1 << 10, 10000);
            tcp.Disconnect();
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
