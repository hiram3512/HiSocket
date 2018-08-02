/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
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
            Connection.OnSend += x => { howManyBytesSend += x.Length; };

        }
    }
}