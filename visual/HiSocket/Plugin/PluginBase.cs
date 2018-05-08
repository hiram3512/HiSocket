/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

namespace HiSocket
{
    public abstract class PluginBase : IPlugin
    {
        public string Name { get; }
        public ITcpConnection Connection { get; }
        public PluginBase(string name, ITcpConnection connection)
        {
            Name = name;
            Connection = connection;
            Connection.AddPlugin(this);
        }
    }
}
