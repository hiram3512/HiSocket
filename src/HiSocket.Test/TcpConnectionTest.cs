using System;
using System.Collections.Generic;
using HiSocket.Tcp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HiSocket.Test
{
    [TestClass]
    public class TcpConnectionTest
    {
        private TcpServer server;

        /// <summary>
        /// Test connection event
        /// </summary>
        [TestMethod]
        public void TestEvent()
        {
            bool isOnSend = false;
            bool isOnReceive = false;

            ITcpConnection tcp = new TcpConnection(new Package());
            tcp.OnSendMessage += (x,y) => { isOnSend = true; };
            tcp.OnReceiveMessage += (x,y) => { isOnReceive = true; };
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnected(tcp);
            tcp.Send(new byte[10]);
            Common.WaitTrue(ref isOnSend);
            Common.WaitTrue(ref isOnReceive);
            Assert.IsTrue(isOnSend);
            Assert.IsTrue(isOnReceive);
        }

        /// <summary>
        /// Test plugin extension
        /// </summary>
        [TestMethod]
        public void TestPlugin()
        {

        }


        [TestMethod]
        public void TestMessageSendAndReceive()
        {
            List<int> sendList = new List<int>();
            List<int> receiveList = new List<int>();
            TcpConnection tcp = new TcpConnection(new Package());
            tcp.OnSendMessage += (x,y) =>
            {
                Console.WriteLine("send:" + BitConverter.ToInt32(y, 0)); sendList.Add(BitConverter.ToInt32(y, 0));
            };
            tcp.OnReceiveMessage += (x,y) =>
            {
                Console.WriteLine("receive:" + BitConverter.ToInt32(y, 0)); receiveList.Add(BitConverter.ToInt32(y, 0));
            };
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnected(tcp);
            for (int i = 0; i < 10000; i++)
            {
                tcp.Send(BitConverter.GetBytes(i));
            }
            //Wait receive data
            Common.WaitListCountEqual(sendList, receiveList);
            for (int i = 0; i < sendList.Count; i++)
            {
                Assert.AreEqual(sendList[i], receiveList[i]);
            }

            tcp.Dispose();
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
