/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using System;

namespace HiSocket
{
    public interface IConnection
    {
        /// <summary>
        /// Trigger when send message
        /// </summary>
        event Action<byte[]> OnSend;//havent packed

        /// <summary>
        /// Trigger when recieve message
        /// </summary>
        event Action<byte[]> OnReceive;//already unpacked

        /// <summary>
        /// Add plugin to extend logic
        /// </summary>
        /// <param name="plugin"></param>
        void AddPlugin(IPlugin plugin);

        /// <summary>
        /// Remove plugin 
        /// </summary>
        /// <param name="name">plugin's name</param>
        void RemovePlugin(string name);

        /// <summary>
        /// Get plugin by name
        /// </summary>
        /// <param name="name">plugin's name</param>
        /// <returns>plugin</returns>
        IPlugin GetPlugin(string name);
    }
}
