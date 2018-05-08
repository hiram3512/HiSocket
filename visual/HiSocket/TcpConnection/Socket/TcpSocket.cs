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

        private IByteBlockBuffer _sendBuffer;
        private IByteBlockBuffer _receiveBuffer;
        private readonly object locker = new object();

        public TcpSocket(int bufferSize = 1 << 16)
        {
            _sendBuffer = new ByteBlockBuffer(bufferSize);
            _receiveBuffer = new ByteBlockBuffer(bufferSize);
        }

        public void Connect(IPEndPoint iep)
        {
            lock (locker)
            {
                if (IsConnected)
                {
                    throw new Exception("Already Connected");
                }
                Assert.NotNull(iep, "IPEndPoint is null");
                ConnectingEvent();
                Socket = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Socket.BeginConnect(iep, delegate (IAsyncResult ar)
                    {
                        try
                        {
                            var socket = ar.AsyncState as Socket;
                            Assert.NotNull(socket, "Socket is null when connect end");
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

        public void Send(byte[] bytes)
        {
            lock (locker)
            {
                if (!IsConnected)
                {
                    throw new Exception("From send : disconnected");
                }
                _sendBuffer.WriteAllBytes(bytes);
                Send();
            }
        }

        private void Send()
        {
            var count = _sendBuffer.Reader.GetHowManyCountCanReadInThisBlock();
            if (count > 0)
            {
                Socket.BeginSend(_sendBuffer.Reader.Node.Value, _sendBuffer.Reader.Position, count, SocketFlags.None,
                    EndSend, Socket);
            }
        }

        private void EndSend(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            Assert.NotNull(socket, "Socket is null when send end");
            int length = socket.EndSend(ar);
            _sendBuffer.Reader.MovePosition(length);
            Send();
        }

        private void Receive()
        {
            var count = _receiveBuffer.Writer.GetHowManyCountCanWriteInThisBlock();
            Socket.BeginReceive(_receiveBuffer.Writer.Node.Value, _receiveBuffer.Writer.Position, count, SocketFlags.None,
                EndReceive, Socket);
        }

        private void EndReceive(IAsyncResult ar)
        {
            var socket = ar.AsyncState as Socket;
            Assert.NotNull(socket, "Socket is null when receive end");
            int length = socket.EndReceive(ar);
            _receiveBuffer.Writer.MovePosition(length);
            var bytes = _receiveBuffer.ReadAllBytes();
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
