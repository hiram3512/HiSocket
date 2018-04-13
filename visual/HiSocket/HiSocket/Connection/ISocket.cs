//****************************************************************************
// Description: 
// Author: hiramtan@live.com
//****************************************************************************

using System;
using System.Net;

namespace HiSocket
{
    /// <summary>
    /// socket api
    /// </summary>
    interface ISocket : ITick
    {
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
        /// trigger when have error
        /// </summary>
        event Action<Exception> OnError;

        /// <summary>
        /// trigger when get message
        /// </summary>
        event Action<byte[]> OnReceive;

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ipe">Server's address</param>
        void Connect(IPEndPoint ipe);

        /// <summary>
        /// Disconnect
        /// </summary>
        void DisConnect();

        
        void Send(byte[] bytes);
    }
}
