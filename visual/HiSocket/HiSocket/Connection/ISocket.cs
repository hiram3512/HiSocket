//****************************************************************************
// Description:
// Author: hiramtan@live.com
//****************************************************************************
using System;

namespace HiSocket
{
    public interface ISocket
    {
        /// <summary>
        /// timeout for connecting server.
        /// default is 5000ms
        /// </summary>
        int TimeOut { get; set; }
        /// <summary>
        /// buffer size for receiving data from server 
        /// default is 1024 * 128; //128k
        /// </summary>
        int ReceiveBufferSize { get; set; }

        /// <summary>
        /// call back when connect state changed 
        /// </summary>
        event Action<SocketState> StateChangeEvent;

        event Action<byte[]> ReceiveEvent;

        /// <summary>
        /// Whole project's tick logic
        /// in main thread
        /// </summary>
        void Run();
        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        void Connect(string ip, int port);
        /// <summary>
        /// 
        /// </summary>
        void DisConnect();
        /// <summary>
        /// send bytes to server
        /// </summary>
        /// <param name="bytes"></param>
        void Send(byte[] bytes);
        /// <summary>
        /// ping time
        /// bug if your unity's .net is 2.0 will thow out a bug.
        /// use .net 4.6 or use unity's ping time.
        /// </summary>
        /// <returns></returns>
        long Ping();
    }
    public enum SocketState
    {
        Connected,
        DisConnected,
        Connecting,
    }
}
