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
        public int HowManyBytesSend;

        public StatisticalPlugin(string name) : base(name)
        {
            TcpConnection.OnSendMessage += (x) => { HowManyBytesSend += x.Length; };
        }
    }
}