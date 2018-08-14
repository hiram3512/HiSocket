/***************************************************************
 * Description: Note: the recommand is TtcpConnection.cs
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using HiFramework;

namespace HiSocket
{
    /// <summary>
    /// socket api
    /// </summary>
    public interface ITcpSocket : IDisposable
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
        /// Send buffer
        /// If disconnect, user can operate the remain data
        /// </summary>
        ICircularBuffer<byte> SendBuffer { get; }

        /// <summary>
        /// Receive buffer
        /// </summary>
        ICircularBuffer<byte> ReceiveBuffer { get; }

        /// <summary>
        /// trigger when connecting
        /// </summary>
        event Action OnConnecting;

        /// <summary>
        /// trigger when connected
        /// </summary>
        event Action OnConnected;

        /// <summary>
        /// trigger when disconnected when user initiate close socket
        /// </summary>
        event Action OnDisconnected;

        /// <summary>
        /// trigger when get message from server, it havent unpacked
        /// use .net socket api
        /// </summary>
        event Action<byte[]> OnSocketReceive;

        /// <summary>
        /// trigger when send message to server, it already packed
        /// use .net socket api
        /// </summary>
        event Action<byte[]> OnSocketSend;

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="iep">server</param>
        void Connect(IPEndPoint iep);

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ip">ipv4/ipv6</param>
        /// <param name="port"></param>
        void Connect(string ip, int port);

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Connect(IPAddress ip, int port);

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        void Send(byte[] bytes);

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        void Send(ArraySegment<byte> bytes);

        /// <summary>
        /// Disconnect
        /// </summary>
        void DisConnect();
    }
}
