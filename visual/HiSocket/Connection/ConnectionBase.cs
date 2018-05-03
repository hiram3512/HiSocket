/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    public abstract class ConnectionBase : IConnection
    {
        public Socket Socket
        {
            get { return ISocket.Socket; }
        }
        public event Action OnConnecting;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<byte[]> OnReceive;////already unpacked
        public event Action<Exception> OnError;
        public event Action OnConstruct;
        public event Action<byte[]> OnSend;//already packed

        protected ISocket ISocket;
        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public ConnectionBase(ISocket socket)
        {
            Assert.NotNull(socket, "ISocket is null");
            ISocket = socket;
            ISocket.OnConnecting += OnConnecting;
            ISocket.OnConnected += OnConnected;
            ISocket.OnDisconnected += OnDisconnected;
            ISocket.OnReceive += OnReceiveFromSocket;
            ISocket.OnError += OnError;
            ConstructEvent();
        }

        public void Connect(IPEndPoint iep)
        {
            Assert.NotNull(iep, "IPEndPoint is null");
            ISocket.Connect(iep);
        }

        public virtual void Send(byte[] bytes)
        {
            ISocket.Send(bytes);
            SendEvent(bytes);
        }

        protected virtual void OnReceiveFromSocket(byte[] bytes)
        {
            ReceiveEvent(bytes);
        }

        public void DisConnect()
        {
            ISocket.OnConnecting -= OnConnecting;
            ISocket.OnConnected -= OnConnected;
            ISocket.OnDisconnected -= OnDisconnected;
            ISocket.OnReceive -= OnReceiveFromSocket;
            ISocket.OnError -= OnError;
            ISocket.DisConnect();
        }

        public void AddPlugin(IPlugin plugin)
        {
            Assert.NotNull(plugin, "plugin is null");
            plugins.Add(plugin.Name, plugin);
        }

        public IPlugin GetPlugin(string name)
        {
            Assert.NotNullOrEmpty(name, "Plugin name is null or empty");
            return plugins[name];
        }

        public void RemovePlugin(string name)
        {
            Assert.NotNullOrEmpty(name, "Plugin name is null or empty");
            plugins.Remove(name);
        }

        void ConstructEvent()
        {
            if (OnConstruct != null)
            {
                OnConstruct();
            }
        }

        void SendEvent(byte[] bytes)
        {
            if (OnSend != null)
            {
                OnSend(bytes);
            }
        }

        void ReceiveEvent(byte[] bytes)
        {
            if (OnReceive != null)
            {
                OnReceive(bytes);
            }
        }
    }
}
