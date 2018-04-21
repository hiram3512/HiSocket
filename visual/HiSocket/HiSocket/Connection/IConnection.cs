/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    interface IConnection : ISocket
    {
        /// <summary>
        /// When construct this will trigger, you can modify logic by yourself.
        /// For exmaple, you can change socket's ipv6 or modify udp's buffer. etc.
        /// </summary>
        event Action OnConstruct;

        /// <summary>
        /// Trigger when send message
        /// </summary>
        event Action<byte[]> OnSend;//already packed

        /// <summary>
        /// Set plugins to extend logic
        /// </summary>
        /// <param name="plugin"></param>
        void SetPlugin(IPlugin plugin);

        /// <summary>
        /// Get plugin
        /// </summary>
        /// <param name="name">plugin's name</param>
        /// <returns>plugin</returns>
        IPlugin GetPlugin(string name);
    }
}
