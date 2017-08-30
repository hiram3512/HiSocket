//****************************************************************************
// Description:
// Author: hiramtan@qq.com
//***************************************************************************

//#define Thread




#if Thread
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket.Tcp
{
    public class TcpClient : ISocket
    {
        public int TimeOut { get; set; }
        public int ReceiveBufferSize { get; set; }
        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get; }

        private System.Net.Sockets.TcpClient tcp;
        private IProto iPackage;
        public void Connect(string ip, int port)
        {
            throw new NotImplementedException();
        }

        public void DisConnect()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public long Ping()
        {
            throw new NotImplementedException();
        }


        private void InitThread()
        {
            Thread sendThread = new Thread(Send);
            sendThread.Start();


        }


        Queue<byte[]> sendQueue = new Queue<byte[]>();
        MemoryStream sendMS = new MemoryStream();
        private bool isSendThreadOn;
        private void Send()
        {
            while (isSendThreadOn)
            {
                if (!IsConnected)//主动or异常断开连接
                    break;

                lock (sendQueue)
                {
                    if (sendQueue.Count > 0)
                    {
                        var msg = sendQueue.Dequeue();
                        sendMS.Seek(0, SeekOrigin.End);
                        sendMS.Write(msg, 0, msg.Length);
                        sendMS.Seek(0, SeekOrigin.Begin);
                        iPackage.Pack(sendMS);
                        var toSend = sendMS.GetBuffer();
                        tcp.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                         {
                             try
                             {
                                 System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                                 tcp.EndConnect(ar);
                             }
                             catch (Exception e)
                             {
                                 Console.WriteLine(e);
                                 throw;
                             }
                         }, tcp);
                    }
                }
            }
        }




    }
}



#else
using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    public class TcpClient : ISocket
    {
        public int TimeOut
        {
            private get { return _timeOut; }
            set { _timeOut = value; }
        }

        public int ReceiveBufferSize
        {
            private get { return _receiveBufferSize; }
            set
            {
                _receiveBufferSize = value;
                _receiveBuffer = new byte[ReceiveBufferSize];
            }
        }

        public Action<SocketState> StateEvent { get; set; }
        public bool IsConnected { get { return _client != null && _client.Client != null && _client.Connected; } }

        private string _ip;
        private int _port;
        private IPackage _iPackage;
        private System.Net.Sockets.TcpClient _client;
        private int _receiveBufferSize = 1024 * 128;//128k
        private byte[] _receiveBuffer;
        private int _timeOut = 5000;//5s:收发超时时间
        private IByteArray _iByteArray_Send = new ByteArray();
        private IByteArray _iByteArray_Receive = new ByteArray();
        public TcpClient(IPackage iPackage)
        {
            _receiveBuffer = new byte[ReceiveBufferSize];
            this._iPackage = iPackage;
            _client = new System.Net.Sockets.TcpClient();
            _client.NoDelay = true;
            _client.SendTimeout = _client.ReceiveTimeout = TimeOut;
        }

        public void Connect(string ip, int port)
        {
            ChangeState(SocketState.Connecting);
            if (IsConnected)
            {
                ChangeState(SocketState.Connected);
                return;
            }
            try
            {
                this._client.BeginConnect(ip, port, (delegate (IAsyncResult ar)
                {
                    try
                    {
                        System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                        tcp.EndConnect(ar);
                        if (tcp.Connected)
                        {
                            ChangeState(SocketState.Connected);
                            tcp.Client.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, tcp);
                        }
                        else ChangeState(SocketState.DisConnected);
                    }
                    catch (Exception e)
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }), _client);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public void Send(byte[] bytes)
        {
            if (!IsConnected)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception("receive failed");
            }
            try
            {
                _iByteArray_Send.Clear();
                _iByteArray_Send.Write(bytes, bytes.Length);
                _iPackage.Pack(_iByteArray_Send);
                var toSend = _iByteArray_Send.Read(_iByteArray_Send.Length);
                _client.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                {
                    try
                    {
                        System.Net.Sockets.TcpClient tcp = ar.AsyncState as System.Net.Sockets.TcpClient;
                        tcp.Client.EndSend(ar);
                    }
                    catch (Exception e)
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception(e.ToString());
                    }
                }, _client);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }


        void Receive(IAsyncResult ar)
        {
            if (!IsConnected)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception("receive failed");
            }
            try
            {
                System.Net.Sockets.TcpClient tcp = ar as System.Net.Sockets.TcpClient;
                int length = tcp.Client.EndReceive(ar);
                if (length > 0)
                {
                    _iByteArray_Receive.Write(_receiveBuffer, length);
                    _iPackage.Unpack(_iByteArray_Receive);
                }
                tcp.Client.BeginReceive(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, Receive, tcp);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public void DisConnect()
        {
            if (IsConnected)
            {
                _client.Client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _client = null;
            }
            ChangeState(SocketState.DisConnected);
            StateEvent = null;
        }
        public long Ping()
        {
            IPAddress ipAddress = IPAddress.Parse(_ip);
            System.Net.NetworkInformation.Ping tempPing = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingReply temPingReply = tempPing.Send(ipAddress);
            return temPingReply.RoundtripTime;
        }


        private void ChangeState(SocketState state)
        {
            if (StateEvent != null)
            {
                StateEvent(state);
            }
        }
    }
}
#endif