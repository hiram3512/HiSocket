/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket.Tcp
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StatisticalPlugin : PluginBase
    {
        /// <summary>
        /// How many bytes total send
        /// </summary>
        private int howManyBytesSend;

        public StatisticalPlugin(string name) : base(name)
        {
            TcpConnection.OnSendMessage += (x, y) => { howManyBytesSend += y.Length; };
        }
    }
}