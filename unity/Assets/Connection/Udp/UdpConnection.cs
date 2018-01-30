//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket
{
    public class UdpConnection : Connection
    {
        private UdpClient _client;

        public override int TimeOut
        {
            get { return _timeOut; }
            set
            {
                _timeOut = value;
                _client.Client.ReceiveTimeout = _client.Client.ReceiveTimeout = TimeOut;
            }
        }

        public override bool IsConnected
        {
            get { return _client != null && _client.Client != null && _client.Client.Connected; }
        }

        public UdpConnection()
        {
            _client = new UdpClient();
            _client.Client.SendTimeout = _client.Client.ReceiveTimeout = TimeOut;
        }

        public override void Connect(string ip, int port)
        {
            if (IsConnected)
            {
                Console.WriteLine("already connected");
                return;
            }
            ChangeState(SocketState.Connecting);
            try
            {
                _client.Client.BeginConnect(ip, port, (x) =>
                {
                    var udp = x.AsyncState as UdpClient;
                    udp.Client.EndConnect(x);
                    if (udp.Client.Connected)
                    {
                        ChangeState(SocketState.Connected);
                        InitThread();
                    }
                    else
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception("udp connected is false");
                    }
                }, _client);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
            }
        }

        public override void DisConnect()
        {
            base.DisConnect();
            if (IsConnected)
            {
                _client.Client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _client = null;
            }
        }

        protected override void Send()
        {
            while (_isSendThreadOn)
            {
                if (!IsConnected) //主动or异常断开连接
                {
                    ChangeState(SocketState.DisConnected);
                    throw new Exception("from send: disconnected");
                }
                if (_sendQueue.Count > 0)
                {
                    lock (_sendQueue)
                    {
                        try
                        {
                            var toSend = _sendQueue.Dequeue();
                            _client.Client.BeginSend(toSend, 0, toSend.Length, SocketFlags.None,
                                delegate (IAsyncResult ar)
                                {
                                    var udp = ar.AsyncState as UdpClient;
                                    var sendLength = udp.Client.EndSend(ar);
                                    if (sendLength != toSend.Length)
                                    {
                                        //todo 待处理sendlength未全部发送
                                        throw new Exception("can not send whole bytes at one time");
                                    }
                                }, _client);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }
                }
            }
        }

        protected override void Receive()
        {
            while (_isReceiveThreadOn)
            {
                if (!IsConnected)
                {
                    ChangeState(SocketState.DisConnected);
                    throw new Exception("from receive: disconnected");
                }
                if (_client.Available > 0)
                {
                    lock (_receiveQueue)
                    {
                        try
                        {
                            _client.Client.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, (x) =>
                            {
                                var udp = x.AsyncState as UdpClient;
                                int length = udp.Client.EndReceive(x);
                                if (length > 0)
                                {
                                    byte[] receiveBytes = new byte[length];
                                    Array.Copy(ReceiveBuffer, 0, receiveBytes, 0, length);
                                    _receiveQueue.Enqueue(receiveBytes);
                                }
                            }, _client);
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.ToString());
                        }
                    }
                }
            }
        }
    }
}