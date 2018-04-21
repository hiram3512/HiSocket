using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HiSocket.Connection
{
    class ConnectionBase : IConnection, ITick
    {
        public Socket Socket { get; }
        public event Action OnConnecting;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<byte[]> OnReceive;
        public event Action<Exception> OnError;

        public ConnectionBase(Socket socket)
        {
            Socket = socket;
            new  Socket(Socket);
            ConstructEvent();
        }
        public void Connect(IPEndPoint iep)
        {
            
        }

        public void Send(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public void DisConnect()
        {
            throw new NotImplementedException();
        }

        public void Tick(float time)
        {
            throw new NotImplementedException();
        }

        public event Action OnConstruct;
        public event Action OnSend;
        public void SetPlugin(IPlugin plugin)
        {
            throw new NotImplementedException();
        }

        public IPlugin GetPlugin(string name)
        {
            throw new NotImplementedException();
        }

        void ConstructEvent()
        {
            if (OnConstruct != null)
            {
                ConstructEvent();
            }
        }
    }
}
