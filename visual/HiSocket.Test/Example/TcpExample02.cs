/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket.Tcp;

namespace HiSocketExample
{
    class TcpExample02
    {
        void Init()
        {
            var tcp = new TcpConnection(new PackageExample());
            tcp.AddPlugin(new PingPlugin("ping", tcp));
            //tcp.GetPlugin("ping");
        }
    }
}
