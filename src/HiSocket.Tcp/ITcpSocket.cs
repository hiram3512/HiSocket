/***************************************************************
 * Description: Note: the recommand is tcpConnection.cs
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiFramework;
using System;
using System.Net;

namespace HiSocket.Tcp
{
    /// <summary>
    /// socket api
    /// </summary>
    public interface ITcpSocket : IDisposable
    {
        /// <summary>
        /// Get socket and modify it(for example: set timeout)
        /// </summary>
        System.Net.Sockets.Socket Socket { get; }

        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Send buffer
        /// If disconnect, user can operate the remain data
        /// </summary>
        IBlockBuffer<byte> SendBuffer { get; }

        /// <summary>
        /// Receive buffer
        /// </summary>
        IBlockBuffer<byte> ReceiveBuffer { get; }

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
        /// trigger when get bytes from server
        /// use .net socket api
        /// </summary>
        event Action<byte[]> OnReceiveBytes;

        /// <summary>
        /// trigger when send bytes to server
        /// use .net socket api
        /// </summary>
        event Action<byte[]> OnSendBytes;

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
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        void Send(byte[] source, int index, int length);

        /// <summary>
        /// Disconnect
        /// </summary>
        void Disconnect();
    }
}
