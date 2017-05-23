//*********************************************************************
// Description:目前主线程收发数据
// Author: hiramtan@live.com
//*********************************************************************

//#define MultipleThread
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket.TCP
{
    public class ClientTcp : Singleton<ClientTcp>, ISocket
    {
        public bool IsConnected { get { return client != null && client.Client != null && client.Connected; } }

        public int bufferSize = 8 * 1024 * 16;//16k is default setting
        public int timeOut = 5000;//5s:收发超时时间

        private TcpClient client;
        private IPAddress address;
        private int port;
        private byte[] buffer;
        private MsgHandler msgHandler;

        private Thread sendThread;
        private Thread receiveThread;

        public ClientTcp()
        {
            if (Socket.OSSupportsIPv6)
                client = new TcpClient(AddressFamily.InterNetworkV6);
            else
                client = new TcpClient(AddressFamily.InterNetwork);

            client.NoDelay = true;
            client.SendTimeout = client.ReceiveTimeout = timeOut;

            buffer = new byte[bufferSize];
            msgHandler = new MsgHandler(this);
        }

        /// <summary>
        /// 开始连接服务器(异步连接)
        /// </summary>
        /// <param name="paramAddress">连接服务器域名(强烈推荐域名)</param>
        /// <param name="paramPort">连接端口</param>
        /// <param name="paramEventHandler">连接成功后的回调事件(可空)</param>
        public bool Connect(string paramAddress, int paramPort)
        {
            address = GetIPAddress(paramAddress);
            port = paramPort;
            bool tempIsConnectSuccess = false;
            try
            {
                client.BeginConnect(address, port, new AsyncCallback(delegate (IAsyncResult ar)
                {
                    try
                    {
                        TcpClient tempTcpClient = (TcpClient)ar.AsyncState;
                        tempTcpClient.EndConnect(ar);
                        tempIsConnectSuccess = ar.IsCompleted;
                        if (tempIsConnectSuccess) ConnectSuccess();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
                }), client);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return tempIsConnectSuccess;
        }

        public long Ping()
        {
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(address);
            return temPingReply.RoundtripTime;
        }

        private IPAddress GetIPAddress(string param)
        {
            IPAddress[] temp = Dns.GetHostAddresses(param);
            if (temp.Length >= 2)
                throw new Exception("this domain links to multiple ip, please check server dns");
            if (temp[0] != null)
                return temp[0];
            throw new Exception("Cannt find this domain's ip address");
        }

        private void ConnectSuccess()
        {
#if MultipleThread
            InitThread();
#else
            client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), client);
#endif
        }

        public bool Send(byte[] param)
        {
#if MultipleThread
            sendQueue.Enqueue(param);
#else
            bool tempIsSendSuccess = false;
            if (!IsConnected)
            {
                throw new Exception("msg send failed, please make sure you have already connected");
            }
            client.Client.BeginSend(param, 0, param.Length, SocketFlags.None, new AsyncCallback(delegate (IAsyncResult ar)
            {
                try
                {
                    TcpClient tempTcpClient = (TcpClient)ar.AsyncState;
                    tempTcpClient.Client.EndSend(ar);
                    tempIsSendSuccess = ar.IsCompleted;
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }), client);
            return tempIsSendSuccess;
#endif
        }

        void SendMsg(byte[] param)
        {

        }

        private void Receive(IAsyncResult ar)
        {
            if (!IsConnected)
            {
                throw new Exception("receive failed, please make sure you have already connected");
            }
            try
            {
                TcpClient tempTcpClient = (TcpClient)ar.AsyncState;
                int temp = tempTcpClient.Client.EndReceive(ar);
                if (temp > 0)
                {
                    msgHandler.Receive(buffer, temp);
                    Array.Clear(buffer, 0, buffer.Length);
                    client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), client);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        public void Close()
        {
#if MultipleThread
            AbortThread();
#endif
            if (IsConnected)
            {
                client.Client.Shutdown(SocketShutdown.Both);
                client.Close();
                client = null;
            }
        }


        void InitThread()
        {
            sendThread = new Thread(SendThread);
            sendThread.IsBackground = true;
            sendThread.Start();

            receiveThread = new Thread(ReceiveThread);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        private bool isSendThreadRunning;
        Queue<byte[]> sendQueue = new Queue<byte[]>();
        private readonly object sendLocker = new object();
        private void SendThread()
        {
            while (isSendThreadRunning)
            {
                lock (sendLocker)//main thread will add data to queue
                {
                    if (sendQueue.Count > 0)
                    {
                        Send(sendQueue.Dequeue());
                    }
                }
            }
        }
        private void ReceiveThread()
        {
            client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), client);
        }

        void AbortThread()
        {
            isSendThreadRunning = false;
            sendThread.Abort();
            sendThread.Join();

            receiveThread.Abort();
            receiveThread.Join();

            while (sendThread.IsAlive || receiveThread.IsAlive)
            {
                Thread.Sleep(100);
            }
            sendQueue.Clear();
            sendThread = null;
            receiveThread = null;
        }
    }
}