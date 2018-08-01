/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using HiFramework;

namespace HiSocket
{
    public class UdpSocket : IUdpSocket
    {
        public Socket Socket { get; private set; }
        public event Action<byte[]> OnSocketReceive;
        public int BufferSize { get; }
        private byte[] buffer;
        public UdpSocket(int bufferSize = 1 << 16)
        {
            BufferSize = bufferSize;
            buffer = new byte[BufferSize];
        }

        public void Connect(IPEndPoint iep)
        {
            AssertThat.IsNotNull(iep);
            Socket = new Socket(iep.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                Socket.BeginConnect(iep, x =>
                {
                    try
                    {
                        var socket = x.AsyncState as Socket;
                        AssertThat.IsNotNull(socket);
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

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ip">ipv4/ipv6</param>
        /// <param name="port"></param>
        public void Connect(string ip, int port)
        {
            var iep = new IPEndPoint(IPAddress.Parse(ip), port);
            Connect(iep);
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(IPAddress ip, int port)
        {
            var iep = new IPEndPoint(ip, port);
            Connect(iep);
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
                        AssertThat.IsNotNull(socket);
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
                Socket.BeginReceive(buffer, 0, BufferSize, SocketFlags.None, ReceiveEnd, Socket);
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
                AssertThat.IsNotNull(socket);
                int length = socket.EndReceive(ar);
                byte[] bytes = new byte[length];
                Array.Copy(buffer, 0, bytes, 0, bytes.Length);
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
            if (OnSocketReceive != null)
            {
                OnSocketReceive(bytes);
            }
        }
    }
}