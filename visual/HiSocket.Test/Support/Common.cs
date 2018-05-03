using System.Net;
using System.Threading;
using HiSocket;
using HiSocketExample;

namespace HiSocketTest
{
    class Common
    {
        public static IPEndPoint GetIpEndPoint()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            return new IPEndPoint(ipAddress, 7777);
        }

        public static TcpConnection GetTcp()
        {
            return new TcpConnection(new PackageExample());
        }

        public static void WaitConnect(ITcp tcp, int timeOut = 1000)
        {
            int time = 0;
            while (!tcp.IsConnected && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }
    }
}
