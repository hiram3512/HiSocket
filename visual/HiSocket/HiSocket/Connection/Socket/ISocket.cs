//****************************************************************************
// Description: 
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket
{
    /// <summary>
    /// socket api
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// Get socket and modify it(for example: set timeout)
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// trigger when connecting
        /// </summary>
        event Action OnConnecting;

        /// <summary>
        /// trigger when connected
        /// </summary>
        event Action OnConnected;

        /// <summary>
        /// trigger when disconnected
        /// </summary>
        event Action OnDisconnected;

        /// <summary>
        /// trigger when get message
        /// </summary>
        event Action<byte[]> OnReceive;

        /// <summary>
        /// trigger when have error(for example: tcp already connect and connect again)
        /// </summary>
        event Action<string> OnError;

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="iep">server</param>
        void Connect(IPEndPoint iep);

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        void Send(byte[] bytes);

        /// <summary>
        /// Disconnect
        /// </summary>
        void DisConnect();
    }
}
