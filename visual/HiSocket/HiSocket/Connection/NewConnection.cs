using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HiSocket
{
    public abstract class NewConnection : ISocket
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

        public Socket Socket { get; }
        public event Action OnConnected;
        public event Action OnConnecting;
        public event Action OnDisconnected;
        public event Action<Exception> OnError;
        public event Action<byte[]> OnReceive;
        public event Action<string> OnWarnning;

        protected NewConnection(Socket socket)
        {
            Socket = socket;
            MakeSureNotNull(Socket, "Socket is null when construct");
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

        protected abstract void Send();
        protected abstract void Receive();

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
        protected void MakeSureNotNull(object obj, string info)
        {
            if (obj == null)
            {
                throw new NullReferenceException("Null exception: " + info);
            }
        }

        protected void ConnectingEvent()
        {
            if (OnConnecting != null)
            {
                OnConnecting();
            }
        }
        protected void ErrorEvent(Exception e)
        {
            if (OnError != null)
            {
                OnError(e);
            }
        }

        protected void ConnectedEvent()
        {
            if (OnConnected != null)
            {
                OnConnected();
            }
        }
    }
}
