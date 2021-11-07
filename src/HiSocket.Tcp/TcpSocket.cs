using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    class TcpSocket : ITcpSocket
    {

        public System.Net.Sockets.Socket Socket { get; private set; }

        //CircularBuffer<byte[]> SendBuffer { get; }

        //CircularBuffer<byte[]> ReceiveBuffer { get; }

        /// <summary>
        /// trigger when connecting
        /// </summary>
        public event Action OnConnecting;

        /// <summary>
        /// trigger when connected
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// Trigger when disconnecte
        /// </summary>
        public event Action OnDisconnected;


        public event Action<Exception> OnError;

        public IBlockBuffer<byte> SendBuffer { get;  set; }

        public IBlockBuffer<byte> ReceiveBuffer { get;  set; }

        /// <summary>
        /// trigger when get bytes from server
        /// use .net socket api
        /// </summary>
        public event Action<IBlockBuffer<byte>, int, int> OnReceiveBytes;

        /// <summary>
        /// trigger when send bytes to server
        /// use .net socket api
        /// </summary>
        public event Action<IBlockBuffer<byte>, int, int> OnSendBytes;

        public TcpSocket(int bufferSize = 1 << 16)
        {
            SendBuffer = new BlockBuffer<byte>(bufferSize);
            ReceiveBuffer = new BlockBuffer<byte>(bufferSize);
        }

        public void Connect(IPEndPoint iep)
        {
            if (iep == null)
            {
                ErrorEvent(new ArgumentNullException("iep is null"));
                return;
            }
            ConnectingEvent();
            try
            {
                Socket = new System.Net.Sockets.Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(iep, EndConnect, Socket);
            }
            catch (Exception e)
            {
                ErrorEvent(e);
                return;
            }
        }

        private void EndConnect(IAsyncResult ar)
        {
            var socket = ar.AsyncState as System.Net.Sockets.Socket;
            if (socket == null)
            {
                ErrorEvent(new ArgumentNullException("socket is null"));
                return;
            }
            try
            {
                socket.EndConnect(ar);
            }
            catch (Exception e)
            {
                ErrorEvent(e);
                return;
            }
            ConnectedEvent();
            ReceiveBytes();
        }


        public void Connect(string ip, int port)
        {
            if (String.IsNullOrEmpty(ip))
            {
                ErrorEvent(new ArgumentNullException("ip is null or empty"));
                return;
            }
            var iep = new IPEndPoint(IPAddress.Parse(ip), port);
            Connect(iep);
        }

        public void Connect(IPAddress ip, int port)
        {
            if (ip == null)
            {
                ErrorEvent(new ArgumentNullException("iep is null"));
                return;
            }
            var iep = new IPEndPoint(ip, port);
            Connect(iep);
        }

        public void ConnectWWW(string www, int port)
        {
            if (String.IsNullOrEmpty(www))
            {
                ErrorEvent(new ArgumentNullException("www is null or empty"));
                return;
            }
            var hostEntry = Dns.GetHostEntry(www);
            if (hostEntry.AddressList == null)
            {
                ErrorEvent(new ArgumentNullException("AddressList is null"));
                return;
            }
            if (hostEntry.AddressList.Length == 0)
            {
                ErrorEvent(new ArgumentException("AddressList length is 0"));
                return;
            }
            IPEndPoint ipe = new IPEndPoint(hostEntry.AddressList[0], port);
            Connect(ipe);
        }

        public void SendBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                ErrorEvent(new ArgumentNullException("bytes is null"));
                return;
            }
            SendBytes(bytes, 0, bytes.Length);
        }

        public void SendBytes(byte[] bytes, int index, int length)
        {
            if (bytes == null)
            {
                ErrorEvent(new ArgumentNullException("bytes is null"));
                return;
            }
            if (index < 0)
            {
                ErrorEvent(new ArgumentException("index<0"));
                return;
            }
            if (length > bytes.Length)
            {
                ErrorEvent(new ArgumentException("length>bytes.Length"));
                return;
            }
            if (Socket == null)
            {
                ErrorEvent(new ArgumentNullException("socket is null"));
                return;
            }
            if (Socket.Connected)
            {
                try
                {
                    //write into buffer, maybe cannot finish send all at one time, resend them
                    SendBuffer.WriteEnd(bytes, index, length);
                    Socket.BeginSend(bytes, index, length, SocketFlags.None, EndSend, Socket);
                }
                catch (Exception e)
                {
                    ErrorEvent(e);
                }
            }
            else
            {
                DisconnectedEvent();
            }
        }
        private void EndSend(IAsyncResult ar)
        {
            if (Socket.Connected)
            {
                var socket = ar.AsyncState as System.Net.Sockets.Socket;
                if (socket == null)
                {
                    ErrorEvent(new ArgumentNullException("socket is null"));
                    return;
                }
                int length = 0;
                try
                {
                    length = socket.EndSend(ar);
                }
                catch (Exception e)
                {
                    ErrorEvent(e);
                    return;
                }
                if (length > 0)
                {
                    SendBytesEvent(0, length);
                    SendBuffer.RemoveFront(length);
                    int remain = SendBuffer.GetCurrentCapcity();
                    if (remain > 0)
                    {
                        int index = SendBuffer.Index;
                        int capcity = SendBuffer.GetCurrentCapcity();
                        SendBytes(SendBuffer.Buffer, index, capcity);
                    }
                }
                else
                {
                    DisconnectedEvent();
                }
            }
            else
            {
                DisconnectedEvent();
            }
        }

        public void Disconnect()
        {
            if (Socket == null)
            {
                ErrorEvent(new ArgumentNullException("socket is null"));
                return;
            }
            try
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception e)
            {
                ErrorEvent(e);
            }
            DisconnectedEvent();
        }

        public void Dispose()
        {
            Disconnect();
        }

        private void ReceiveBytes()
        {
            if (Socket.Connected)
            {
                try
                {
                    int index = ReceiveBuffer.Index;
                    int count = ReceiveBuffer.GetCurrentCapcity();
                    Socket.BeginReceive(ReceiveBuffer.Buffer, index, count, SocketFlags.None, EndReceive, Socket);
                }
                catch (Exception e)
                {
                    ErrorEvent(e);
                    return;
                }
            }
            else
            {
                DisconnectedEvent();
            }
        }

        private void EndReceive(IAsyncResult ar)
        {
            if (Socket.Connected)
            {
                var socket = ar.AsyncState as System.Net.Sockets.Socket;
                if (socket == null)
                {
                    ErrorEvent(new ArgumentNullException("socket is null"));
                    return;
                }
                int length = 0;
                try
                {
                    length = socket.EndReceive(ar);
                }
                catch (Exception e)
                {
                    ErrorEvent(e);
                    return;
                }
                if (length > 0)
                {
                    ReceiveBuffer.IncreaseIndex(length);
                    ReceiveBytesEvent(0, ReceiveBuffer.Index);
                    ReceiveBytes();
                }
                else
                {
                    DisconnectedEvent();
                }
            }
            else
            {
                DisconnectedEvent();
            }
        }

        private void ConnectingEvent()
        {
            if (OnConnecting != null)
            {
                OnConnecting();
            }
        }

        private void ConnectedEvent()
        {
            if (OnConnected != null)
            {
                OnConnected();
            }
        }

        private void DisconnectedEvent()
        {
            if (OnDisconnected != null)
            {
                OnDisconnected();
            }
        }

        private void ErrorEvent(Exception ex)
        {
            if (OnError != null)
            {
                OnError(ex);
            }
        }

        private void SendBytesEvent(int index, int length)
        {
            if (OnSendBytes != null)
            {
                OnSendBytes(SendBuffer, index, length);
            }
        }

        private void ReceiveBytesEvent(int index, int length)
        {
            if (OnReceiveBytes != null)
            {
                OnReceiveBytes(ReceiveBuffer, index, length);
            }
        }
    }
}
