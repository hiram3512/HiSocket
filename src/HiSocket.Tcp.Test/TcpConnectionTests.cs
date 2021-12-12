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
    public class TcpConnectionTests
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
        public void SendTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            byte[] byte1 = new byte[] { 1 };
            byte[] byte2 = null;
            var package = new Example.ExamplePackage2();
            var tcp = new TcpConnection(package);
            tcp.OnSendMessage += (data) =>
              {
                  byte2 = data;
                  evt.Set();
              };
            tcp.OnConnected += () =>
            {
                tcp.Send(byte1);
            };
            tcp.Connect("127.0.0.1", 999);
            evt.WaitOne();
            Assert.AreEqual(byte1[0], byte2[0]);
        }

        [TestMethod()]
        public void ReceiveTest()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            byte[] byte1 = new byte[] { 1 };
            byte[] byte2 = null;
            var package = new Example.ExamplePackage2();
            var tcp = new TcpConnection(package);
            tcp.OnReceiveMessage += (data) =>
            {
                byte2 = data;
                evt.Set();
            };

            tcp.OnConnected += () =>
            {
                tcp.Send(byte1);
            };
            tcp.Connect("127.0.0.1", 999);
            evt.WaitOne();
            Assert.AreEqual(byte1[0], byte2[0]);
        }
    }
}