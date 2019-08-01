using HiSocket.Message;
using HiSocket.Tcp;

namespace HiSocket.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            ITcpConnection tcp = new TcpConnection(new Package());
            tcp.OnSendMessage += (x, y) => { };
            tcp.OnReceiveMessage += (x, y) => { };
            tcp.Connect("127.0.0.1", 7777);
            tcp.Send(new byte[10]);
        }
    }
}
