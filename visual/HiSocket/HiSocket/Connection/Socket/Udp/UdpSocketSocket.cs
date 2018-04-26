/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    public class UdpSocketSocket : SocketBase, IUdpSocket
    {
        public int BufferSize
        {
            get { return _buffer.Length; }
            set { _buffer = new byte[value]; }
        }

        private byte[] _buffer;

        public override void Connect(IPEndPoint iep)
        {
            Assert.NotNull(iep, "IPEndPoint is null");
            ConnectingEvent();
            Socket = new Socket(iep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                Socket.BeginConnect(iep, x =>
                {
                    try
                    {
                        var socket = x.AsyncState as Socket;
                        Assert.NotNull(socket, "Socket is null when connect end");
                        if (!Socket.Connected)
                        {
                            throw new Exception("Connect faild");
                        }
                        socket.EndConnect(x);
                        ConnectedEvent();
                        StartReceive();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }

                }, Socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

        }

        public override void Send(byte[] bytes)
        {
            try
            {
                Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, x =>
                {
                    try
                    {
                        var socket = x.AsyncState as Socket;
                        Assert.NotNull(socket, "Socket is null when send end");
                        int length = socket.EndSend(x);
                        //Todo: because this is udp protocol, this is no sence
                        if (length != bytes.Length) { }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }

                }, Socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void StartReceive()
        {
            try
            {
                Socket.BeginReceive(_buffer, 0, BufferSize, SocketFlags.None, ReceiveEnd, Socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void ReceiveEnd(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                Assert.NotNull(socket, "Socket is null when receive end");
                int length = socket.EndReceive(ar);
                byte[] bytes = new byte[length];
                Array.Copy(_buffer, 0, bytes, 0, bytes.Length);
                ReceiveEvent(bytes);
                StartReceive();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}