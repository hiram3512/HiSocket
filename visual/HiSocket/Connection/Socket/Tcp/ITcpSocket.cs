/***************************************************************
 * Description: tcp's api 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface ITcpSocket : ISocket
    {
        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }
    }
}