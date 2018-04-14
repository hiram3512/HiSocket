/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket
{
    public abstract class SocketBase : ISocket
    {
        /// <summary>
        /// For send data
        /// </summary>
        private Thread sendThread;
        /// <summary>
        /// For receive data
        /// </summary>
        private Thread receiveThread;

        /// <summary>
        /// If send thread is run
        /// </summary>
        protected bool IsSendThreadOn;
        /// <summary>
        /// If receive thread is run
        /// </summary>
        protected bool IsReceiveThreadOn;

        public Socket Socket { get; private set; }
        public event Action OnConnected;
        public event Action OnConnecting;
        public event Action OnDisconnected;
        public event Action<byte[]> OnReceive;
        public event Action<string> OnError;

        protected SocketBase(Socket socket)
        {
            Socket = socket;
            Assert.IsNotNull(Socket, "Socket is null when construct");
        }

        public abstract void Connect(IPEndPoint iep);

        protected void InitThread()
        {
            IsSendThreadOn = true;
            sendThread = new Thread(Send);
            sendThread.Start();
            IsReceiveThreadOn = true;
            receiveThread = new Thread(Receive);
            receiveThread.Start();
        }

        private void AbortThread()
        {
            try
            {
                IsReceiveThreadOn = false;
                if (sendThread != null)
                    sendThread.Abort();
                sendThread = null;
            }
            catch (Exception e)
            {
                throw new Exception("Abort send thread with error: " + e.ToString());
            }
            try
            {
                IsReceiveThreadOn = false;
                if (receiveThread != null)
                    receiveThread.Abort();
                receiveThread = null;
            }
            catch (Exception e)
            {
                throw new Exception("Abort receive thread with error: " + e.ToString());
            }
        }

        protected abstract void Send();
        protected abstract void Receive();

        public void DisConnect()
        {
            AbortThread();
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
                OnError(info);
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
