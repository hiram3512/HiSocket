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
    public class TcpSocket : ITcpSocket
    {
        public Socket Socket { get; private set; }
        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }
        public event Action OnConnecting;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<byte[]> OnSocketReceive;

        private IByteBlockBuffer sendBuffer;
        private IByteBlockBuffer receiveBuffer;
        private readonly object locker = new object();

        public TcpSocket(int bufferSize = 1 << 16)
        {
            sendBuffer = new ByteBlockBuffer(bufferSize);
            receiveBuffer = new ByteBlockBuffer(bufferSize);
        }

        public void Connect(IPEndPoint iep)
        {
            lock (locker)
            {
                if (IsConnected)
                {
                    throw new Exception("Already Connected");
                }
                AssertThat.IsNotNull(iep);
                ConnectingEvent();
                Socket = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Socket.BeginConnect(iep, delegate (IAsyncResult ar)
                    {
                        try
                        {
                            var socket = ar.AsyncState as Socket;
                            AssertThat.IsNotNull(socket);
                            socket.EndConnect(ar);
                            if (!IsConnected)
                            {
                                throw new Exception("Connect faild");
                            }
                            ConnectedEvent();
                            Receive();
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
            lock (locker)
            {
                if (!IsConnected)
                {
                    throw new Exception("From send : disconnected");
                }
                sendBuffer.WriteAllBytes(bytes);
                Send();
            }
        }

        private void Send()
        {
            var count = sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
            if (count > 0)
            {
                Socket.BeginSend(sendBuffer.Reader.Node.Value, sendBuffer.Reader.Position, count, SocketFlags.None,
                    EndSend, Socket);
            }
        }

        private void EndSend(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            AssertThat.IsNotNull(socket);
            int length = socket.EndSend(ar);
            sendBuffer.Reader.MovePosition(length);
            Send();
        }

        private void Receive()
        {
            var count = receiveBuffer.Writer.GetHowManyCountCanWriteInThisBlock();
            Socket.BeginReceive(receiveBuffer.Writer.Node.Value, receiveBuffer.Writer.Position, count, SocketFlags.None,
                EndReceive, Socket);
        }

        private void EndReceive(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            AssertThat.IsNotNull(socket);
            int length = socket.EndReceive(ar);
            receiveBuffer.Writer.MovePosition(length);
            var bytes = receiveBuffer.ReadAllBytes();
            SocketReceiveEvent(bytes);
            if (length > 0)
            {
                Receive();
            }
        }

        public void DisConnect()
        {
            lock (locker)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                DisconnectedEvnet();
            }
        }
        void ConnectingEvent()
        {
            if (OnConnecting != null)
            {
                OnConnecting();
            }
        }
        void ConnectedEvent()
        {
            if (OnConnected != null)
            {
                OnConnected();
            }
        }
        void SocketReceiveEvent(byte[] bytes)
        {
            if (OnSocketReceive != null)
            {
                OnSocketReceive(bytes);
            }
        }
        private void DisconnectedEvnet()
        {
            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
        }
    }
}
