/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket.Tcp
{
    public interface IPlugin
    {
        /// <summary>
        /// Plugins name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin belong to this tcpConnection
        /// </summary>
        ITcpConnection TcpConnection { get; set; }
    }
}
