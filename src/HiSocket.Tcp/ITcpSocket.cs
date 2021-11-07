using System;
using System.Collections.Generic;
using System.Text;
using CircularBuffer;
using System.Net;

namespace HiSocket.Tcp
{
    interface ITcpSocket : IDisposable
    {
        System.Net.Sockets.Socket Socket { get; }

        /// <summary>
        /// trigger when connecting
        /// </summary>
        event Action OnConnecting;

        /// <summary>
        /// trigger when connected
        /// </summary>
        event Action OnConnected;

        /// <summary>
        /// Trigger when disconnecte
        /// </summary>
        event Action OnDisconnected;

         IBlockBuffer<byte> SendBuffer { get; set; }

         IBlockBuffer<byte> ReceiveBuffer { get; set; }

        /// <summary>
        /// trigger when get bytes from server
        /// use .net socket api
        /// </summary>
        event Action<IBlockBuffer<byte>, int, int> OnReceiveBytes;

        /// <summary>
        /// trigger when send bytes to server
        /// use .net socket api
        /// </summary>
        event Action<IBlockBuffer<byte>, int, int> OnSendBytes;

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
        /// Connect to server
        /// </summary>
        /// <param name="www"></param>
        /// <param name="port"></param>
        void ConnectWWW(string www, int port);

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
