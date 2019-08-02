using HiSocket.Tcp;

namespace HiSocket.Example
{
    class Example1
    {
        static void Main(string[] args)
        {

        }

        TcpConnection tcp;
        void Connect()
        {
            var package = new PackageExample();
            tcp = new TcpConnection(package);
            //tcp.OnConnecting+=
            //tcp.OnDisconnected+=
            tcp.OnConnected += OnConnected;
            tcp.OnReceiveMessage += OnReceive;
            tcp.Connect("127.0.0.1", 999);
        }

        void OnConnected()
        {
            //connect success
            tcp.Send(new byte[10]);
        }

        void OnReceive(byte[] message)
        {

        }
    }
}
