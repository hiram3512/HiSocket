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
    public class UdpConnection : UdpSocket, IConnection
    {
        public event Action OnConstruct;
        public event Action<byte[]> OnSend;
        public event Action<byte[]> OnReceive;

        private Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();
        public UdpConnection(int bufferSize = 1 << 16) : base(bufferSize)
        {
            ConstructEvent();
            OnSocketReceive += OnSocketReceiveHandler;
        }

        public new void Send(byte[] bytes)
        {
            base.Send(bytes);
            SendEvent(bytes);
        }

        public void AddPlugin(IPlugin plugin)
        {
            Assert.IsNotNull(plugin);
            _plugins.Add(plugin.Name, plugin);
        }

        public IPlugin GetPlugin(string name)
        {
            Assert.IsNotNullOrEmpty(name);
            return _plugins[name];
        }

        public void RemovePlugin(string name)
        {
            Assert.IsNotNullOrEmpty(name);
            _plugins.Remove(name);
        }
        void ConstructEvent()
        {
            if (OnConstruct != null)
            {
                OnConstruct();
            }
        }
        void OnSocketReceiveHandler(byte[] bytes)
        {
            ReceiveEvent(bytes);
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
