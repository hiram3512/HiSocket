/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket;

namespace HiSocketExample
{
    class PluginExample
    {
        void Init()
        {
            var tcp = new TcpConnection(new PackageExample());
            tcp.AddPlugin(new PingPlugin("ping"));
        }
    }
}
