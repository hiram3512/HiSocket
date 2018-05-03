/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;

namespace HiSocket.Tcp
{
    /// <summary>
    /// socket api
    /// </summary>
    public interface ITcpSocket
    {
        /// <summary>
        /// Get socket and modify it(for example: set timeout)
        /// </summary>
        Socket Socket { get; }
        
        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }

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
        event Action<byte[]> OnSocketReceive;

        /// <summary>
        /// trigger when have error(for example: tcp already connect and connect again)
        /// </summary>
        event Action<Exception> OnError;

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
