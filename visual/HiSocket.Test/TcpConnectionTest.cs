using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiSocket.Test
{
    [TestClass]
    public class TcpConnectionTest
    {
        private TcpServer server;
        /// <summary>
        /// initialize server
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            server = new TcpServer();
        }

        /// <summary>
        /// Clean up server
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            if (server != null)
                server.Close();
            server = null;
        }

        [TestMethod]
        public void TestCreate()
        {
            var tcp = Common.GetTcpConnection();
            Assert.IsNotNull(tcp);
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnect(tcp);
            Assert.IsTrue(tcp.IsConnected);
        }
    }
}
