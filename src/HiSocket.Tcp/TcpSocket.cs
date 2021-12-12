/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    public class TcpSocket : ITcpSocket
    {
        public System.Net.Sockets.Socket Socket { get; private set; }

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

        /// <summary>
        /// when exception
        /// </summary>
        public event Action<Exception> OnException;

        public IBlockBuffer<byte> SendBuffer { get; set; }

        public IBlockBuffer<byte> ReceiveBuffer { get; set; }

        /// <summary>
        /// trigger when get bytes from server
        /// use .net socket api
        /// </summary>
        public event Action<IBlockBuffer<byte>> OnReceiveBytes;

        /// <summary>
        /// trigger when send bytes to server
        /// use .net socket api
        /// </summary>
        public event Action<byte[]> OnSendBytes;

        public TcpSocket(int bufferSize = 1 << 16)
        {
            SendBuffer = new BlockBuffer<byte>(bufferSize);
            ReceiveBuffer = new BlockBuffer<byte>(bufferSize);
        }

        public void Connect(EndPoint endPoint)
        {
            ConnectingEvent();
            try
            {
                Socket = new System.Net.Sockets.Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(endPoint, ConnectCallback, Socket);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as System.Net.Sockets.Socket;
                socket.EndConnect(ar);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
            ConnectedEvent();
            ReceiveBytes();
        }


        public void Connect(string ip, int port)
        {
            IPEndPoint iep = null;
            try
            {
                iep = new IPEndPoint(IPAddress.Parse(ip), port);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
            Connect(iep);
        }

        public void Connect(IPAddress ip, int port)
        {
            IPEndPoint iep = null;
            try
            {
                iep = new IPEndPoint(ip, port);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
            Connect(iep);
        }

        public void ConnectWWW(string www, int port)
        {
            IPEndPoint iep = null;
            try
            {
                var hostEntry = Dns.GetHostEntry(www);
                iep = new IPEndPoint(hostEntry.AddressList[0], port);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
            Connect(iep);
        }

        public void SendBytesInBuffer()
        {
            try
            {
                if (SendBuffer.Index > 0)
                {
                    Socket.BeginSend(SendBuffer.Buffer, 0, SendBuffer.Index, SocketFlags.None, SendCallback, Socket);
                }
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
            }
        }

        public void SendBytes(byte[] bytes)
        {
            try
            {
                SendBytes(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
        }

        public void SendBytes(byte[] bytes, int index, int length)
        {
            try
            {
                //write into buffer, maybe cannot finish send all at one time, resend them
                SendBuffer.WriteAtEnd(bytes, index, length);
                SendBytesInBuffer();
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
            }
        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as System.Net.Sockets.Socket;
                int length = 0;
                length = socket.EndSend(ar);
                if (length > 0)
                {
                    byte[] data = SendBuffer.ReadFromHead(length);
                    SendBytesEvent(data);
                    int remain = SendBuffer.Index;
                    if (remain > 0)
                    {
                        SendBytes(SendBuffer.Buffer, 0, SendBuffer.Index);
                    }
                }
                else
                {
                    DisconnectedEvent();
                }
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
            }
        }

        public void Disconnect()
        {
            try
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
            DisconnectedEvent();
        }

        public void Dispose()
        {
            Disconnect();
            SendBuffer.Dispose();
            SendBuffer = null;
            ReceiveBuffer.Dispose();
            ReceiveBuffer = null;
        }

        private void ReceiveBytes()
        {
            try
            {
                int index = ReceiveBuffer.Index;
                int count = ReceiveBuffer.GetCurrentCapcity();
                Socket.BeginReceive(ReceiveBuffer.Buffer, index, count, SocketFlags.None, ReceiveCallback, Socket);
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
                return;
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as System.Net.Sockets.Socket;
                int length = 0;
                length = socket.EndReceive(ar);
                if (length > 0)
                {
                    ReceiveBuffer.IncreaseIndex(length);
                    ReceiveBytesEvent(ReceiveBuffer);
                    ReceiveBytes();
                }
                else
                {
                    DisconnectedEvent();
                }
            }
            catch (Exception e)
            {
                ExceptionEvent(e);
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

        protected void ExceptionEvent(Exception ex)
        {
            if (OnException != null)
            {
                OnException(ex);
            }
        }

        private void SendBytesEvent(byte[] data)
        {
            if (OnSendBytes != null)
            {
                OnSendBytes(data);
            }
        }

        private void ReceiveBytesEvent(IBlockBuffer<byte> receiveBuffer)
        {
            if (OnReceiveBytes != null)
            {
                OnReceiveBytes(receiveBuffer);
            }
        }
    }
}
