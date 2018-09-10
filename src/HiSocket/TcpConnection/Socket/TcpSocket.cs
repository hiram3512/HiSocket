/***************************************************************
 * Description: Note: the recommand is use tcpconnection
 * This is the baisc logic of socket
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
        /// <summary>
        /// Get socket and modify it(for example: set timeout)
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// if connected
        /// </summary>
        public bool IsConnected
        {
            get { return Socket != null && Socket.Connected; }
        }

        /// <summary>
        /// trigger when connecting
        /// </summary>
        public event Action OnConnecting;

        /// <summary>
        /// trigger when connected
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// trigger when disconnected when user initiate close socket
        /// </summary>
        public event Action OnDisconnected;

        /// <summary>
        /// trigger when get message from server, it havent unpacked
        /// use .net socket api
        /// </summary>
        public event Action<byte[]> OnSocketReceive;

        /// <summary>
        /// trigger when send message to server, it already packed
        /// use .net socket api
        /// </summary>
        public event Action<byte[]> OnSocketSend;

        /// <summary>
        /// Send buffer
        /// If disconnect, user can operate the remain data
        /// </summary>
        public ICircularBuffer<byte> SendBuffer { get; private set; }

        /// <summary>
        /// Receive buffer
        /// </summary>
        public ICircularBuffer<byte> ReceiveBuffer { get; private set; }

        private readonly object locker = new object();

        /// <summary>
        /// The default buffer is 1<<16, if small will automatically add buffer block
        /// </summary>
        /// <param name="bufferSize"></param>
        public TcpSocket(int bufferSize = 1 << 16)
        {
            SendBuffer = new CircularBuffer<byte>(bufferSize);
            ReceiveBuffer = new CircularBuffer<byte>(bufferSize);
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        public void Connect(IPEndPoint iep)
        {
            lock (locker)
            {
                AssertThat.IsFalse(IsConnected, "Already Connected");
                AssertThat.IsNotNull(iep, "iep is null");
                ConnectingEvent();
                try
                {
                    Socket = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                //Start connect
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
            AssertThat.IsNotNullOrEmpty(ip);
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
            AssertThat.IsNotNull(ip);
            var iep = new IPEndPoint(ip, port);
            Connect(iep);
        }

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            lock (locker)
            {
                if (!IsConnected)
                {
                    throw new Exception("From send : disconnected");
                }
                SendBuffer.Write(bytes);//use for geting havent send successfull data
                try
                {
                    Socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, EndSend, Socket);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(ArraySegment<byte> bytes)
        {
            Send(bytes.Array);
        }

        private void EndSend(IAsyncResult ar)
        {
            //User disconnect connection proactively
            if (!IsConnected)
            {
                return;
            }
            int length = 0;
            try
            {
                var socket = ar.AsyncState as Socket;
                AssertThat.IsNotNull(socket);
                length = socket.EndSend(ar);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            byte[] sendBytes = new byte[length];
            Array.Copy(SendBuffer.Array, SendBuffer.ReadPosition, sendBytes, 0, sendBytes.Length);
            SocketSendEvent(sendBytes);
            SendBuffer.MoveReadPosition(length);
        }

        private void Receive()
        {
            var count = ReceiveBuffer.EState == CircularBuffer<byte>.State.WriteAhead
                ? ReceiveBuffer.Size - ReceiveBuffer.WritePosition
                : ReceiveBuffer.ReadPosition - ReceiveBuffer.WritePosition;
            try
            {
                Socket.BeginReceive(ReceiveBuffer.Array, ReceiveBuffer.WritePosition, count, SocketFlags.None,
                    EndReceive, Socket);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        private void EndReceive(IAsyncResult ar)
        {
            //User disconnect connection proactively
            if (!IsConnected)
            {
                return;
            }
            int length = 0;
            try
            {
                var socket = ar.AsyncState as Socket;
                AssertThat.IsNotNull(socket);
                length = socket.EndReceive(ar);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            ReceiveBuffer.MoveWritePosition(length);
            var bytes = ReceiveBuffer.ReadAll();
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
                if (IsConnected)
                {
                    try
                    {
                        Socket.Shutdown(SocketShutdown.Both);
                        Socket.Close();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
                    DisconnectedEvnet();
                }
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

        private void SocketSendEvent(byte[] bytes)
        {
            if (OnSocketSend != null)
            {
                OnSocketSend(bytes);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            DisConnect();

            Socket = null;
            OnConnecting = null;
            OnConnected = null;
            OnDisconnected = null;
            OnSocketReceive = null;
            OnSocketSend = null;
            SendBuffer.Dispose();
            ReceiveBuffer.Dispose();
        }
    }
}