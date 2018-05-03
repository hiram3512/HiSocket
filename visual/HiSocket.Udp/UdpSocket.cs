/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Udp
{
    public class UdpSocket : IUdpSocket
    {
        public Socket Socket { get; private set; }
        public event Action<byte[]> OnReceive;
        public event Action<Exception> OnError;
        public int BufferSize { get; }
        private byte[] _buffer;
        public UdpSocket(int bufferSize)
        {
            BufferSize = bufferSize;
            _buffer = new byte[BufferSize];
        }

        public void Connect(IPEndPoint iep)
        {
            Assert.NotNull(iep, "IPEndPoint is null");
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

        public void Send(byte[] bytes)
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

        public void DisConnect()
        {
            Socket.Close();
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

        void ReceiveEvent(byte[] bytes)
        {
            if (OnReceive != null)
            {
                OnReceive(bytes);
            }
        }
    }
}