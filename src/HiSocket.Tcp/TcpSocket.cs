/***************************************************************
 * Description: Note: the recommand is use tcpconnection
 * This is the baisc logic of socket
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiFramework;
using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    public class TcpSocket : ITcpSocket
    {
        /// <summary>
        /// Get socket and modify it(for example: set timeout)
        /// </summary>
        public System.Net.Sockets.Socket Socket { get; private set; }

        /// <summary>
        /// if connected(should use heart beat check if server disconnect)
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
        /// Trigger when disconnected
        /// </summary>
        public event Action OnDisconnected;

        /// <summary>
        /// trigger when get message from server, it havent unpacked
        /// use .net socket api
        /// </summary>
        public event Action<byte[]> OnReceiveBytes;

        /// <summary>
        /// trigger when send message to server, it already packed
        /// use .net socket api
        /// </summary>
        public event Action<byte[]> OnSendBytes;

        /// <summary>
        /// Send buffer
        /// If disconnect, user can operate the remain data
        /// </summary>
        public IBlockBuffer<byte> SendBuffer { get; private set; }

        /// <summary>
        /// Receive buffer
        /// </summary>
        public IBlockBuffer<byte> ReceiveBuffer { get; private set; }

        private readonly object _locker = new object();

        /// <summary>
        /// The default buffer is 1<<16, if small will automatically add buffer block
        /// </summary>
        /// <param name="bufferSize"></param>
        public TcpSocket(int bufferSize = 1 << 16)
        {
            SendBuffer = new BlockBuffer<byte>(bufferSize);
            ReceiveBuffer = new BlockBuffer<byte>(bufferSize);
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        public void Connect(IPEndPoint iep)
        {
            lock (_locker)
            {
                AssertThat.IsFalse(IsConnected, "Already Connected");
                AssertThat.IsNotNull(iep, "iep is null");
                ConnectingEvent();
                try
                {
                    Socket = new System.Net.Sockets.Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                try
                {
                    Socket.BeginConnect(iep, delegate (IAsyncResult ar)
                    {
                        try
                        {
                            var socket = ar.AsyncState as System.Net.Sockets.Socket;
                            AssertThat.IsNotNull(socket, "Socket is null when end connect");
                            socket.EndConnect(ar);
                            if (IsConnected)
                            {
                                ConnectedEvent();
                                Receive();
                            }
                            else
                            {
                                AssertThat.Fail("Connect faild");
                            }
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
            lock (_locker)
            {
                AssertThat.IsNotNullOrEmpty(ip, "ip is null or empty");
                var iep = new IPEndPoint(IPAddress.Parse(ip), port);
                Connect(iep);
            }
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(IPAddress ip, int port)
        {
            lock (_locker)
            {
                AssertThat.IsNotNull(ip, "ip is null");
                var iep = new IPEndPoint(ip, port);
                Connect(iep);
            }
        }

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="www"></param>
        /// <param name="port"></param>
        public void ConnectWWW(string www, int port)
        {
            var hostEntry = Dns.GetHostEntry(www);
            if (hostEntry.AddressList != null && hostEntry.AddressList.Length > 0)
            {
                IPEndPoint ipe = new IPEndPoint(hostEntry.AddressList[0], port);
                Connect(ipe);
            }
            else
            {
                AssertThat.Fail("Check host"); 
            }
        }

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        public void Send(byte[] bytes)
        {
            lock (SendBuffer)
            {
                Send(bytes, 0, bytes.Length);
            }
        }

        public void Send(byte[] bytes, int index, int length)
        {
            lock (SendBuffer)
            {
                AssertThat.IsTrue(IsConnected, "From send : disconnected");
                SendBuffer.Write(bytes, index, length);
                try
                {
                    Socket.BeginSend(bytes, index, length, SocketFlags.None, EndSend, Socket);
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
            }
        }

        private void EndSend(IAsyncResult ar)
        {
            lock (SendBuffer)
            {
                if (IsConnected)//User disconnect tcpConnection proactively
                {
                    int length = 0;
                    try
                    {
                        var socket = ar.AsyncState as System.Net.Sockets.Socket;
                        AssertThat.IsNotNull(socket, "Socket is null when end send");
                        length = socket.EndSend(ar);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
                    byte[] sendBytes = SendBuffer.Read(length);
                    SendBuffer.ResetIndex();
                    SocketSendEvent(sendBytes);
                }
            }
        }

        private void Receive()
        {
            lock (ReceiveBuffer)
            {
                if (IsConnected)
                {
                    try
                    {
                        var count = ReceiveBuffer.Size - ReceiveBuffer.WritePosition;
                        Socket.BeginReceive(ReceiveBuffer.Buffer, ReceiveBuffer.WritePosition, count, SocketFlags.None,
                            EndReceive, Socket);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
                }
            }
        }

        private void EndReceive(IAsyncResult ar)
        {
            lock (ReceiveBuffer)
            {
                if (IsConnected) //User disconnect tcpConnection proactively
                {
                    int length = 0;
                    try
                    {
                        var socket = ar.AsyncState as System.Net.Sockets.Socket;
                        AssertThat.IsNotNull(socket, "Socket is null when end receive");
                        length = socket.EndReceive(ar);
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
                    ReceiveBuffer.MoveWritePosition(length);
                    var bytes = ReceiveBuffer.Read(ReceiveBuffer.WritePosition);
                    ReceiveBuffer.ResetIndex();
                    SocketReceiveEvent(bytes);
                    if (length > 0)
                    {
                        Receive();
                    }
                    else
                    {
                        DisconnectedEvnet();
                    }
                }
            }
        }

        public void Disconnect()
        {
            lock (_locker)
            {
                if (IsConnected)
                {
                    DisconnectedEvnet();
                    try
                    {
                        Socket.Shutdown(SocketShutdown.Both);
                        Socket.Close();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.ToString());
                    }
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
            if (OnReceiveBytes != null)
            {
                OnReceiveBytes(bytes);
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
            if (OnSendBytes != null)
            {
                OnSendBytes(bytes);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            lock (_locker)
            {
                Disconnect();
                Socket = null;
                OnConnecting = null;
                OnConnected = null;
                OnDisconnected = null;
                OnReceiveBytes = null;
                OnSendBytes = null;
                SendBuffer.Dispose();
                ReceiveBuffer.Dispose();
            }
        }
    }
}