using System.Net;
using System.Threading;
using HiSocket;
using HiSocketExample;

namespace HiSocket.Test
{
    class Common
    {
        public static IPEndPoint GetIpEndPoint()
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            return new IPEndPoint(ipAddress, 7777);
        }

        public static TcpConnection GetTcpConnection()
        {
            return new TcpConnection(new PackageExample());
        }

        public static void WaitConnect(ITcpSocket tcp, int timeOut = 1000)
        {
            int time = 0;
            while (!tcp.IsConnected && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }

        public static void WaitTime(int timeOut = 1000)
        {
            int time = 0;
            while (time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }

        public static void WaitValue(ref int value, int result, int timeOut = 1000)
        {
            int time = 0;
            while (value != result && time < timeOut)
            {
                time++;
                Thread.Sleep(1);
            }
        }
    }
}
