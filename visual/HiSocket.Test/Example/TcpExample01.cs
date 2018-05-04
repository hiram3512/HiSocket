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
        private IPackage _package = new PackageExample();
        private TcpConnection _tcp;
        static void Main(string[] args)
        {

        }
        void Init()
        {
            _tcp = new TcpConnection(_package);
            _tcp.OnConnected += OnConnected;
            _tcp.OnReceive += Receive;
            //_tcp.OnError
            //_tcp.OnDisconnected
        }
        void OnConnected()
        {
            //connect success
            _tcp.Send(new byte[10]);//send message
            _tcp.DisConnect();//disconnect
        }

        void Receive(byte[] bytes)
        {
            //get message from server
        }
    }
}
