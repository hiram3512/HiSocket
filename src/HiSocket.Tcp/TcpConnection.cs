/***************************************************************
 * Description: This class for user simply use tcp socket in project
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using HiFramework;

namespace HiSocket.Tcp
{
    public class TcpConnection : TcpSocket, ITcpConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        public event Action<byte[]> OnSendMessage;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        public event Action<byte[]> OnReceiveMessage;


        private readonly IPackage _package;

        private Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

        public TcpConnection(IPackage package)
        {
            this._package = package;
            OnReceiveBytes += SocketReceiveHandler;
        }

        public new void Send(byte[] bytes)
        {
            SendEvent(bytes);
            _package.Pack(bytes, x => { base.Send(x); });
        }

        void SocketReceiveHandler(byte[] bytes)
        {
            _package.Unpack(bytes, x => { ReceiveEvent(x); });
        }

        /// <summary>
        /// Add plugin to extend logic
        /// </summary>
        /// <param name="plugin"></param>
        public void AddPlugin(IPlugin plugin)
        {
            AssertThat.IsNotNull(plugin, "Plugin is null");
            plugin.TcpConnection = this;
            _plugins.Add(plugin.Name, plugin);
        }

        /// <summary>
        /// Get plugin by name
        /// </summary>
        /// <param name="name">plugin's name</param>
        /// <returns>plugin</returns>
        public IPlugin GetPlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name, "Name is null or empty");
            return _plugins[name];
        }

        public bool IsPluginExist(string name)
        {
            AssertThat.IsNotNullOrEmpty(name, "Name is null or empty");
            return _plugins.ContainsKey(name);
        }

        /// <summary>
        /// Remove plugin 
        /// </summary>
        /// <param name="name">plugin's name</param>
        public void RemovePlugin(string name)
        {
            AssertThat.IsNotNullOrEmpty(name, "Name is null or empty");
            _plugins.Remove(name);
        }

        void SendEvent(byte[] bytes)
        {
            if (OnSendMessage != null)
            {
                OnSendMessage(bytes);
            }
        }

        void ReceiveEvent(byte[] bytes)
        {
            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(bytes);
            }
        }
    }
}