/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using HiFramework;

namespace HiSocket
{
    public class UdpConnection : UdpSocket, IConnection
    {
        public event Action<byte[]> OnSend;
        public event Action<byte[]> OnReceive;

        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public UdpConnection(int bufferSize = 1 << 16) : base(bufferSize)
        {
            OnSocketReceive += OnSocketReceiveHandler;
        }

        /// <summary>
        /// To quickly get plugin
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPlugin this[string name]
        {
            get
            {
                return GetPlugin(name);
            }
            set
            {
                AddPlugin(value);
            }
        }

        public new void Send(byte[] bytes)
        {
            base.Send(bytes);
            SendEvent(bytes);
        }

        public void AddPlugin(IPlugin plugin)
        {
            AssertThat.IsNotNull(plugin);
            plugins.Add(plugin.Name, plugin);
        }

        public IPlugin GetPlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name);
            return plugins[name];
        }

        public void RemovePlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name);
            plugins.Remove(name);
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
