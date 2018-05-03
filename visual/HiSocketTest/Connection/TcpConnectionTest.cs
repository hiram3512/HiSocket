using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiSocketTest.Connection
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
                _server.Disconnect();
            _server = null;
        }

        [TestMethod]
        public void TestCreate()
        {
            var tcp = Common.GetTcp();
            Assert.IsNotNull(tcp);
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnect(tcp);
            Assert.IsTrue(tcp.IsConnected);
        }
    }
}
