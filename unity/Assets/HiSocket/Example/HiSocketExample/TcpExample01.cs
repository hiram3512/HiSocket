/***************************************************************
 * Description: 
 *
 * Documents: https://github.com/hiramtan/HiSocket_unity
 * Author: hiramtan@live.com
***************************************************************/

using HiSocket.Tcp;

namespace HiSocketExample
{
    class TcpExample01
    {
        static void Main(string[] args)
        {
            var tcp = new TcpConnection(new PackageExample());
            tcp.OnReceive += Receive;
            tcp.Send(new byte[1]);
        }

        static void Receive(byte[] bytes)
        {
        }
    }
}
