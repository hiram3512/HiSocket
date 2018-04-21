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
    public abstract class SocketBase : ISocket
    {
        public Socket Socket { get; private set; }
        public event Action OnConnected;
        public event Action OnConnecting;
        public event Action OnDisconnected;
        public event Action<byte[]> OnReceive;
        public event Action<Exception> OnError;

        protected SocketBase(Socket socket)
        {
            Socket = socket;
            Assert.NotNull(Socket, "Socket is null when construct");
        }

        public abstract void Connect(IPEndPoint iep);

        public virtual void DisConnect()
        {
            try
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Socket = null;
            }
            catch (Exception e)
            {
                throw new Exception("Disconnect with error: " + e.ToString());
            }
            DisconnectedEvnet();
        }

        public abstract void Send(byte[] bytes);

        protected void ConnectingEvent()
        {
            if (OnConnecting != null)
            {
                OnConnecting();
            }
        }
        protected void ErrorEvent(string info)
        {
            if (OnError != null)
            {
                OnError(new Exception(info));
            }
        }

        protected void ConnectedEvent()
        {
            if (OnConnected != null)
            {
                OnConnected();
            }
        }

        protected void ReceiveEvent(byte[] bytes)
        {
            if (OnReceive != null)
            {
                OnReceive(bytes);
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
