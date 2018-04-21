/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    interface IConnection : ISocket, ITick
    {
        /// <summary>
        /// Get plugin
        /// </summary>
        /// <param name="name">Plugin's name</param>
        /// <returns>plugin</returns>
        IPlugin this[string name] { get; }

        /// <summary>
        /// Get plugin
        /// </summary>
        /// <param name="name">Plugin's name</param>
        /// <returns>plugin</returns>
        IPlugin GetPlugin(string name);

        /// <summary>
        /// SetPlugin
        /// </summary>
        /// <param name="plugin"></param>
        void SetPlugin(IPlugin plugin);
    }
}
