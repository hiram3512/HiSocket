using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    interface ITcpConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        event Action<byte[]> OnSend;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        event Action<byte[]> OnReceive;
    }
}
