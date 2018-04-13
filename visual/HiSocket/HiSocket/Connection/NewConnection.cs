using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    public class NewConnectio : ISocket
    {
        public event Action OnConnected;
        public event Action OnConnecting;
        public event Action OnDisconnected;
        public event Action<Exception> OnError;
        public event Action<byte[]> OnReceive;
        public event Action<string> OnWarnning;

        private Socket _socket;

        public NewConnectio(Socket socket)
        {
            _socket = socket;
            MakeSureNotNull(_socket, "Socket is null when construct");
        }

        public void Connect(IPEndPoint iep)
        {
            MakeSureNotNull(iep, "IPEndPoint is null");
            if (OnConnecting != null)
            {
                OnConnecting();
            }
            _socket.BeginConnect(iep, delegate (IAsyncResult ar)
            {
                var socket = ar.AsyncState as Socket;
                if (socket == null)
                {
                    MakeSureNotNull(socket, "socket is null when end ");
                }




                if (tcp != null && tcp.Connected)
                {
                    tcp.EndConnect(ar);
                    ChangeState(SocketState.Connected);
                    InitThread();
                }
                else
                {
                    ChangeState(SocketState.DisConnected);
                    throw new Exception("socket connect failed");
                }
            }, _socket);
        }

        public void DisConnect()
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void Tick(float time)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Make sure the object is not null
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        private void MakeSureNotNull(object obj, string info)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Null exception: " + info);
            }
        }

        private void ConnectingEvent()
        {
            if (OnConnecting != null)
            {
                OnConnecting();
            }
        }
        private void ErrorEvent(Exception e)
        {
            if (OnError != null)
            {
                OnError(e);
            }
        }
    }
}
