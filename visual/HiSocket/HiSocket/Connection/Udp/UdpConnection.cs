/****************************************************************
 * Description: 
 *
 * Author: hiramtan@live.com
 *////////////////////////////////////////////////////////////////////////

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    public class UdpConnection : Connection, IUdp
    {
        public override void Connect(string ip, int port)
        {
            var address = Dns.GetHostAddresses(ip)[0];
            _socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                _socket.BeginConnect(ip, port, delegate (IAsyncResult ar)
                {
                    var socket = ar.AsyncState as Socket;
                    socket.EndConnect(ar);
                    InitThread();
                }, _socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        protected override void Send()
        {
            while (_isSendThreadOn)
            {
                if (_sendQueue.Count > 0)
                {
                    lock (_sendQueue)
                    {
                        var toSend = _sendQueue.Dequeue();
                        try
                        {
                            _socket.BeginSend(toSend, 0, toSend.Length, SocketFlags.None, delegate (IAsyncResult ar)
                            {
                                var socket = ar.AsyncState as Socket;
                                var sendLength = socket.EndSend(ar);
                                if (sendLength != toSend.Length)
                                {
                                    //todo: if this will happend, msdn is not handle this issue
                                    throw new Exception("error: protocol is udp");
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
                                Array.Copy(ReceiveBuffer, 0, receiveBytes, 0, length);
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