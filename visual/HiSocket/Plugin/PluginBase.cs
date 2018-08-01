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
        public string Name { get; }
        public IConnection Connection { get; }
        public PluginBase(string name, IConnection connection)
        {
            Name = name;
            Connection = connection;
            Connection.AddPlugin(this);
        }
    }
}
