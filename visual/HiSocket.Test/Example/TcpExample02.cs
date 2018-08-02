/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket;

namespace HiSocketExample
{
    class TcpExample02
    {
        void Init()
        {
            var tcp = new TcpConnection(new PackageExample());
            tcp.AddPlugin( new PingPlugin("Ping"));
            //tcp.GetPlugin("ping");
        }
    }
}
