using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiSocket.Test
{
    [TestClass]
    public class TcpConnectionTest
    {
        private TcpServer _server;
        /// <summary>
        /// initialize server
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _server = new TcpServer();
        }

        /// <summary>
        /// Clean up server
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            if (_server != null)
                _server.Close();
            _server = null;
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
