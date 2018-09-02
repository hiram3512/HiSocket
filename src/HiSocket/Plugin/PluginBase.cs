/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public abstract class PluginBase : IPlugin
    {
        /// <summary>
        /// Plugins name
        /// </summary>
        public string Name { get; set; }
        public IConnection Connection { get; set; }

        public PluginBase(string name)
        {
            Name = name;
        }
    }
}
