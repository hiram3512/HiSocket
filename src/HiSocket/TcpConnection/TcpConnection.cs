/***************************************************************
 * Description: This class for user simply use tcp socket in project
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiFramework;
using System;
using System.Collections.Generic;

namespace HiSocket
{
    public class TcpConnection : TcpSocket, IConnection
    {
        private IPackage package;

        private readonly IByteArray send = new ByteArray();

        private readonly IByteArray receive = new ByteArray();

        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();

        /// <summary>
        /// Trigger when send message
        /// </summary>
        public event Action<byte[]> OnSend;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        public event Action<byte[]> OnReceive;

        public TcpConnection(IPackage package)
        {
            this.package = package;
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
            send.Write(bytes);
            SendEvent(bytes);
            package.Pack(send, x => { base.Send(x); });
        }
        void OnSocketReceiveHandler(byte[] bytes)
        {
            receive.Write(bytes);
            package.Unpack(receive, x => { ReceiveEvent(x); });
        }

        /// <summary>
        /// Add plugin to extend logic
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(IPlugin plugin)
        {
            AssertThat.IsNotNull(plugin);
            plugin.Connection = this;
            plugins.Add(plugin.Name, plugin);
        }

        /// <summary>
        /// Get plugin by name
        /// </summary>
        /// <param name="name">plugin's name</param>
        /// <returns>plugin</returns>
        public IPlugin GetPlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name);
            return plugins[name];
        }

        /// <summary>
        /// Remove plugin 
        /// </summary>
        /// <param name="name">plugin's name</param>
        public void RemovePlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name);
            plugins.Remove(name);
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