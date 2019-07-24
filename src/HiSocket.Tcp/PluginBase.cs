/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket.Tcp
{
    public abstract class PluginBase : IPlugin
    {
        /// <summary>
        /// Plugins name
        /// </summary>
        public string Name { get; set; }
        public ITcpConnection TcpConnection { get; set; }

        public PluginBase(string name)
        {
            Name = name;
        }
    }
}
