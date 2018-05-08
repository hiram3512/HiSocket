/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class TcpConnection : TcpSocket, ITcpConnection
    {
        private IPackage _iPackage;
        private readonly IByteArray _send = new ByteArray();
        private readonly IByteArray _receive = new ByteArray();
        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public event Action OnConstruct;
        public event Action<byte[]> OnSend;
        public event Action<byte[]> OnReceive;
        public TcpConnection(IPackage package)
        {
            _iPackage = package;
            OnSocketReceive += OnSocketReceiveHandler;
            ConstructEvent();
        }

        public new void Send(byte[] bytes)
        {
            _send.Write(bytes);
            _iPackage.Pack(_send, x =>
            {
                SendEvent(x);
                base.Send(x);
            });
        }
        protected void OnSocketReceiveHandler(byte[] bytes)
        {
            _receive.Write(bytes);
            _iPackage.Unpack(_receive, x => { ReceiveEvent(x); });
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