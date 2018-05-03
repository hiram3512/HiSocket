/***************************************************************
 * Description: tcp's api 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public interface ITcp : ISocket
    {
        /// <summary>
        /// if connected
        /// </summary>
        bool IsConnected { get; }
    }
}