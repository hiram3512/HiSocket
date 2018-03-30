/****************************************************************
 * Description: 
 * UDP Sockets are "connection-less", so the protocol does not know anything about whether or not the server and client are connected.
 *
 * Author: hiramtan@live.com
 *////////////////////////////////////////////////////////////////////////

using System;

namespace HiSocket
{
    public interface IUdp
    {
        event Action<byte[]> ReceiveEvent;
        /// <summary>
        ///  tick logic
        /// in main thread
        /// </summary>
        void Run();

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
        /// use .net 4.6 or use unity's ping time instead.
        /// </summary>
        /// <returns></returns>
        long Ping(string ip);
    }
}