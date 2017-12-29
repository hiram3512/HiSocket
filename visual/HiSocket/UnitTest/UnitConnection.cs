using System;
using HiSocket;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTest
{
    [TestClass]
    public class UnitConnection
    {
        [TestMethod]
        public void TestTcpConnect()
        {
            TcpConnection connection = new TcpConnection(new TestPackage());
            connection.StateChangeHandler = TestConnectHandler;
           connection.Connect("",12);
        }
        [TestMethod]
        public void TestUdpConnect()
        {

        }
        [TestMethod]
        public void TestConnectHandler(SocketState state)
        {
            Assert.IsTrue(state==SocketState.Connected);
        }
    }
}
