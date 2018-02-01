//****************************************************************************
// Description:
// Author: hiramtan@live.com
//***************************************************************************
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket
{
    public class UdpConnection : Connection
    {
        public override void Connect(string ip, int port)
        {
            if (IsConnected)
            {
                Console.WriteLine("already connected");
                return;
            }
            ChangeState(SocketState.Connecting);
            var address = Dns.GetHostAddresses(ip)[0];
            _socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            _socket.NoDelay = true;
            _socket.SendTimeout = _socket.ReceiveTimeout = TimeOut;
            try
            {
                _socket.BeginConnect(ip, port, delegate (IAsyncResult ar)
                {
                    var tcp = ar.AsyncState as Socket;
                    if (tcp != null && tcp.Connected)
                    {
                        tcp.EndConnect(ar);
                        ChangeState(SocketState.Connected);
                        InitThread();
                    }
                    else
                    {
                        ChangeState(SocketState.DisConnected);
                        throw new Exception("tcp connected is false");
                    }
                }, _socket);
            }
            catch (Exception e)
            {
                ChangeState(SocketState.DisConnected);
                throw new Exception(e.ToString());
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
                        var toSend = _sendQueue.Dequeue();
                        try
                        {
                            _socket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                            {
                                var tcp = ar.AsyncState as Socket;
                                var sendLength = tcp.EndSend(ar);
                                if (sendLength != toSend.Length)
                                {
                                    //todo: if this will happend, msdn is not handle this issue
                                    throw new Exception("can not send whole bytes at one time");
                                }
                            }, _socket);
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
                if (_socket.Available > 0)
                {
                    try
                    {
                        _socket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, delegate (IAsyncResult ar)
                        {
                            var tcp = ar.AsyncState as Socket;
                            int length = tcp.EndReceive(ar);
                            if (length > 0)
                            {
                                byte[] receiveBytes = new byte[length];
                                Array.Copy(ReceiveBuffer,0,receiveBytes,0,length);
                                lock (_receiveQueue)
                                {
                                    _receiveQueue.Enqueue(receiveBytes);
                                }
                            }
                        }, _socket);
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