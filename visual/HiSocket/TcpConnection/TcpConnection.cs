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
    public class TcpConnection : TcpSocket, IConnection
    {
        private IPackage package;
        private readonly IByteArray send = new ByteArray();
        private readonly IByteArray receive = new ByteArray();
        private Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public event Action OnConstruct;
        public event Action<byte[]> OnSend;
        public event Action<byte[]> OnReceive;
        public TcpConnection(IPackage package)
        {
            this.package = package;
            OnSocketReceive += OnSocketReceiveHandler;
            ConstructEvent();
        }

        public new void Send(byte[] bytes)
        {
            send.Write(bytes);
            package.Pack(send, x =>
            {
                SendEvent(x);
                base.Send(x);
            });
        }
        void OnSocketReceiveHandler(byte[] bytes)
        {
            receive.Write(bytes);
            package.Unpack(receive, x => { ReceiveEvent(x); });
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