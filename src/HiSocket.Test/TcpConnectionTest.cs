using System;
using System.Collections.Generic;
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

            TcpConnection tcp = new TcpConnection(new Package());
            tcp.OnSend += (x) => { isOnSend = true; };
            tcp.OnReceive += (x) => { isOnReceive = true; };
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
            tcp.OnSend += x =>
            {
                Console.WriteLine("send:" + BitConverter.ToInt32(x, 0)); sendList.Add(BitConverter.ToInt32(x, 0));
            };
            tcp.OnReceive += x => { Console.WriteLine("receive:" + BitConverter.ToInt32(x, 0)); receiveList.Add(BitConverter.ToInt32(x, 0)); };
            tcp.Connect(Common.GetIpEndPoint());
            Common.WaitConnected(tcp);
            for (int i = 0; i < 10000; i++)
            {
                tcp.Send(BitConverter.GetBytes(i));
            }
            //Wait receive data
            Common.WaitListCountEqual(sendList, receiveList, 10000);
            for (int i = 0; i < sendList.Count; i++)
            {
                Assert.AreEqual(sendList[i], receiveList[i]);
            }

            tcp.Dispose();
        }


        public class Package : IPackage
        {
            /// <summary>
            /// 在此处理接收到服务器数据后的拆包粘包
            /// </summary>
            /// <param name="bytes"></param>
            public void Unpack(IByteArray source, Action<byte[]> unpackedHandler)
            {
                while (source.Length >= 4)
                {
                    //read a int
                    var @byte = source.Read(4);
                    unpackedHandler(@byte);
                }
            }

            /// <summary>
            /// 在此处理将要发送的数据添加长度消息id等
            /// </summary>
            /// <param name="bytes"></param>
            public void Pack(IByteArray source, Action<byte[]> packedHandler)
            {
                var @byte = source.Read(4);
                packedHandler(@byte);
            }
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
