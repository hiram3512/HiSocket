/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Net;

namespace HiSocket.Tcp
{
    /// <summary>
    /// Tcp socket interface
    /// </summary>
    public interface ITcpSocket : IDisposable
    {
        /// <summary>
        /// Socket instance
        /// </summary>
        System.Net.Sockets.Socket Socket { get; }

        /// <summary>
        /// when connecting
        /// </summary>
        event Action OnConnecting;

        /// <summary>
        /// when connected
        /// </summary>
        event Action OnConnected;

        /// <summary>
        /// when disconnecte
        /// </summary>
        event Action OnDisconnected;

        /// <summary>
        /// when exception
        /// </summary>
        event Action<Exception> OnException;

        /// <summary>
        /// 
        /// </summary>
        IBlockBuffer<byte> SendBuffer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IBlockBuffer<byte> ReceiveBuffer { get; set; }

        /// <summary>
        /// trigger when get bytes from server
        /// use .net socket api
        /// </summary>
        event Action<IBlockBuffer<byte>> OnReceiveBytes;

        /// <summary>
        /// trigger when send bytes to server
        /// use .net socket api
        /// </summary>
        event Action<byte[]> OnSendBytes;

        /// <summary>
        /// Connect to server
        /// </summary>
        /// <param name="iep">server</param>
        void Connect(EndPoint iep);

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
        /// Connect to server
        /// </summary>
        /// <param name="www"></param>
        /// <param name="port"></param>
        void ConnectWWW(string www, int port);


        /// <summary>
        /// Send buffer data
        /// </summary>
        void SendBytesInBuffer();

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        void SendBytes(byte[] bytes);

        /// <summary>
        /// Send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        void SendBytes(byte[] bytes, int index, int length);

        /// <summary>
        /// Disconnect
        /// </summary>
        void Disconnect();
    }
}
