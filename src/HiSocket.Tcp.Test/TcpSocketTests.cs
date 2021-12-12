/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiSocket.Tcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;

namespace HiSocket.Tcp.Test
{
    [TestClass()]
    public class TcpSocketTests
    {
        private TestServer server;

        [TestInitialize]
        public void Init()
        {
            server = new TestServer();
        }

        [TestCleanup]
        public void Cleanup()
        {
            server.Close();
            server = null;
        }

        [TestMethod()]
        public void TcpSocketTest()
        {
            var tcp1 = new TcpSocket();
            Assert.IsNotNull(tcp1);
            var tcp2 = new TcpSocket(1024);
            Assert.IsNotNull(tcp2);
        }

        [TestMethod()]
        public void ConnectTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            var isConnected = false;
            var isConnecting = false;
            var tcp = new TcpSocket();
            tcp.OnConnecting += () =>
            {
                isConnecting = true;
            };
            tcp.OnConnected += () =>
            {
                isConnected = true;
                evt.Set();
            };
            tcp.OnDisconnected += () =>
            {
                isConnected = false;
                evt.Set();
            };
            tcp.Connect("127.0.0.1", 999);
            evt.WaitOne();
            Assert.IsTrue(isConnecting);
            Assert.IsTrue(isConnected);

            tcp.Disconnect();
            evt.WaitOne();
            Assert.IsFalse(isConnected);
        }

        [TestMethod()]
        public void SendBytesTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            byte[] byte1 = new byte[] { 123 };
            byte[] byte2 = null;
            var tcp = new TcpSocket();
            tcp.OnSendBytes += (data) =>
            {
                byte2 = data;
                evt.Set();
            };
            tcp.OnConnected += () =>
            {
                tcp.SendBytes(byte1);
            };
            tcp.Connect("127.0.0.1", 999);
            evt.WaitOne();
            Assert.AreEqual(byte1[0], byte2[0]);
        }

        [TestMethod()]
        public void ReceiveBytesTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            byte[] byte1 = new byte[] { 1, 2 };
            byte[] byte2 = null;
            var tcp = new TcpSocket();
            tcp.OnReceiveBytes += (buffer) =>
            {
                var receiveLength = buffer.Index;
                byte2 = buffer.ReadFromHead(receiveLength);
                evt.Set();
            };
            tcp.OnConnected += () =>
            {
                tcp.SendBytes(byte1);
            };
            tcp.Connect("127.0.0.1", 999);
            evt.WaitOne();
            Assert.AreEqual(byte1[0], byte2[0]);
            Assert.AreEqual(byte1[1], byte2[1]);
        }

        [TestMethod()]
        public void DisposeTest()
        {
            var tcp = new TcpSocket();
            tcp.Dispose();
            Assert.IsNull(tcp.SendBuffer);
            Assert.IsNull(tcp.ReceiveBuffer);
        }

        [TestMethod()]
        public void ExceptionTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            Exception testException = null;
            var tcp = new TcpSocket();
            tcp.OnException += (e) =>
            {
                testException = e;
                evt.Set();
            };
            tcp.Connect("123", 999);
            evt.WaitOne();
            Assert.IsNotNull(testException);
        }
    }
}