/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    class PluginBase : IPlugin
    {
        public string Name { get; }

        public PluginBase(string name)
        {
            Name = name;
        }
    }
}
