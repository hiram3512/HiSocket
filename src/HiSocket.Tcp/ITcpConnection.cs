/***************************************************************
 * Description: Block buffer for reuse array
 * 
 * Documents: https://github.com/hiram3512/HiSocket
 * Support: hiramtan@live.com
***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace HiSocket.Tcp
{
    /// <summary>
    /// Tcp connection
    /// </summary>
    public interface ITcpConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        event Action<byte[]> OnSendMessage;

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        event Action<byte[]> OnReceiveMessage;

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="data"></param>
        void Send(byte[] data);
    }
}
